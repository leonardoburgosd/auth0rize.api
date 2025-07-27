using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Domain.Queries.DomainGet
{
    internal class DomainGetHandler : IRequestHandler<DomainGet, Response<DomainGetResponse>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public DomainGetHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        #endregion 

        public async Task<Response<DomainGetResponse>> Handle(DomainGet request, CancellationToken cancellationToken)
        {
            Response<DomainGetResponse> response = new Response<DomainGetResponse>();

            var domain = await _unitOfWork.Repository<domain.Domain.Domain>().QueryPagedAsync<domain.Domain.Domain>(
                filters: new Dictionary<string, object> {
                    { "Code", request.search is not null ? new Guid(request.search) : null },
                    { "IsDeleted", request.state is not null ? (request.state.Equals("active") ? false : true) : null }
                },
                skip: request.page - 1,
                take: request.size,
                orderBy: "RegistrationDate",
                ascending: true,
                schema: Schemas.Security
                );
            var users = await _unitOfWork.Repository<domain.User.User>().QueryAsync<domain.User.User>(schema: Schemas.Security);
            var usersDomain = await _unitOfWork.Repository<domain.UserDomain.UserDomain>().QueryAsync<domain.UserDomain.UserDomain>(schema: Schemas.Security);

            response.Success = true;
            response.Message = "Lista de dominios completa.";
            response.Data = new DomainGetResponse()
            {
                Total = domain.data.Count(),
                Page = request.page,
                Active = domain.data.Where(d => d.IsDeleted == false).Count(),
                Deleted = domain.data.Where(d => d.IsDeleted == true).Count(),
                Domains = domain.data.Select(d => new DomainListResponse()
                {
                    Code = d.Code.ToString(),
                    PrincipalEmail = users.FirstOrDefault(u => u.Id == d.UserRegistration)?.Email ?? "No encontrado",
                    PrincipalName = users.FirstOrDefault(u => u.Id == d.UserRegistration)?.FirstName + " " + users.FirstOrDefault(u => u.Id == d.UserRegistration)?.LastName
                                    ?? "No encontrado",
                    Count = usersDomain.Where(ud => ud.DomainId == d.Id).Count(),
                    IsActive = !d.IsDeleted,
                }).ToList()
            };

            return response;
        }
    }
}
