using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Queries.ApplicationGet
{
    internal class ApplicationGetHandler : IRequestHandler<ApplicationGet, Response<ApplicationGetResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationGetHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<ApplicationGetResponse>> Handle(ApplicationGet request, CancellationToken cancellationToken)
        {
            var (data, total) = await _unitOfWork.Repository<domain.Application.Application>().QueryPagedAsync<domain.Application.Application>(
                filters: new Dictionary<string, object>
                {
                    { "Name", request.Search },
                    { "IsDeleted", false }
                },
                orderBy: "Name",
                ascending: true,
                skip: request.Page - 1,
                take: request.Size,
                includeDeleted: false,
                useLikeFilter: request.Search != null,
                schema: Schemas.Organization
            );

            var list = data.Select(a => new ApplicationListItem
            {
                Id = a.Id,
                Name = a.Name,
                Code = a.Code.ToString(),
                Description = a.Description,
                Avatar = a.Avatar,
                IsDeleted = a.IsDeleted,
                RegistrationDate = a.RegistrationDate.ToString("dd-MM-yyyy HH:mm:ss")
            }).ToList();

            return new Response<ApplicationGetResponse>(new ApplicationGetResponse
            {
                Total = total,
                Page = request.Page,
                Applications = list
            }, "Lista de aplicaciones obtenida correctamente.");
        }
    }
}
