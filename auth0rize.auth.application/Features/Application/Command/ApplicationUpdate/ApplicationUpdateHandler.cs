using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Command.ApplicationUpdate
{
    internal class ApplicationUpdateHandler : IRequestHandler<ApplicationUpdate, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationUpdateHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(ApplicationUpdate request, CancellationToken cancellationToken)
        {
            var appList = await _unitOfWork.Repository<domain.Application.Application>().QueryAsync<domain.Application.Application>(
                new Dictionary<string, object> { { "Id", request.Id } },
                Schemas.Organization
            );

            var app = appList.FirstOrDefault();
            if (app == null) throw new ApiException("Aplicación no encontrada.");

            // Validar nombre único (excluyendo el propio registro)
            var existing = await _unitOfWork.Repository<domain.Application.Application>().QueryAsync<domain.Application.Application>(
                new Dictionary<string, object> { { "Name", request.Name } },
                Schemas.Organization
            );
            if (existing.Any(a => a.Id != app.Id)) throw new ApiException("Ya existe otra aplicación con ese nombre.");

            app.Name = request.Name;
            app.Description = request.Description;
            app.Avatar = request.Avatar;
            app.DateUpdate = DateTime.Now;
            app.UserUpdate = request.UserUpdate;

            await _unitOfWork.Repository<domain.Application.Application>().UpdateAsync(app, Schemas.Organization);

            return new Response<bool>(true, "Aplicación actualizada correctamente.");
        }
    }
}
