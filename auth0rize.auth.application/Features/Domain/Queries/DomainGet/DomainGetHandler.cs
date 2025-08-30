using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;
using System.Text.RegularExpressions;

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

            if (request.search is not null)
            {
                bool isValid = Regex.IsMatch(request.search, @"^[a-fA-F0-9]{8}\-[a-fA-F0-9]{4}\-[a-fA-F0-9]{4}\-[a-fA-F0-9]{4}\-[a-fA-F0-9]{12}$");
                if (!isValid) throw new ApiException("El formato de código tiene al menos 32 caracteres.");
            }

            var domain = await _unitOfWork.Repository<domain.Domain.Domain>().QueryPagedAsync<domain.Domain.Domain>(
                filters: new Dictionary<string, FilterOption>
                {
                    ["Code"] = new FilterOption { Value = request.search is not null ? new Guid(request.search) : null, Operator = FilterOperator.Equals },
                    ["IsDeleted"] = new FilterOption { Value = request.state is not null ? (request.state.Equals("active") ? false : true) : null, Operator = FilterOperator.Equals }
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
                    Initial = users.FirstOrDefault(u => u.Id == d.UserRegistration)?.FirstName.Substring(0, 1).ToUpper() + users.FirstOrDefault(u => u.Id == d.UserRegistration)?.LastName.Substring(0, 1).ToUpper()
                                    ?? "No encontrado",
                    Count = usersDomain.Where(ud => ud.DomainId == d.Id).Count(),
                    IsActive = !d.IsDeleted,
                    Users = usersDomain.Where(u => u.UserId != d.UserRegistration && !u.IsDeleted)
                                        .Select(u => new DomainUsersResponse()
                                        {
                                            Id = u.UserId,
                                            Email = users.Where(us => us.Id == u.UserId).FirstOrDefault().Email ?? "",
                                            Name = $"{users.Where(us => us.Id == u.UserId).FirstOrDefault().FirstName ?? ""} {users.Where(us => us.Id == u.UserId).FirstOrDefault().LastName ?? ""}"
                                        }).ToList()
                }).ToList()
            };

            return response;
        }
    }
}
