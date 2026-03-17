using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Command.ApplicationDelete
{
    internal class ApplicationDeleteHandler : IRequestHandler<ApplicationDelete, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationDeleteHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(ApplicationDelete request, CancellationToken cancellationToken)
        {
            var appList = await _unitOfWork.Repository<domain.Application.Application>().QueryAsync<domain.Application.Application>(
                new Dictionary<string, object> { { "Id", request.Id } },
                Schemas.Organization
            );

            if (!appList.Any()) throw new ApiException("Aplicación no encontrada.");

            // Soft delete usando el método estándar del repositorio
            await _unitOfWork.Repository<domain.Application.Application>().DeleteAsync<domain.Application.Application>(
                request.Id, request.UserDeleted, Schemas.Organization
            );

            return new Response<bool>(true, "Aplicación eliminada correctamente.");
        }
    }
}
