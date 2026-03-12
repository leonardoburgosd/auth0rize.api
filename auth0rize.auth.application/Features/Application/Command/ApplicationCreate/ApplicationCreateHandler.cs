using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Command.ApplicationCreate
{
    internal class ApplicationCreateHandler : IRequestHandler<ApplicationCreate, Response<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationCreateHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(ApplicationCreate request, CancellationToken cancellationToken)
        {
            var existing = await _unitOfWork.Repository<domain.Application.Application>().QueryAsync<domain.Application.Application>(
                new Dictionary<string, object> { { "Name", request.Name } },
                Schemas.Organization
            );

            if (existing.Any()) throw new ApiException("Ya existe una aplicación con ese nombre.");

            var id = await _unitOfWork.Repository<domain.Application.Application>().InsertAsync(new domain.Application.Application
            {
                Name = request.Name,
                Description = request.Description,
                Avatar = request.Avatar,
                UserRegistration = request.UserRegistration
            }, Schemas.Organization);

            return new Response<int>(id, "Aplicación creada correctamente.");
        }
    }
}
