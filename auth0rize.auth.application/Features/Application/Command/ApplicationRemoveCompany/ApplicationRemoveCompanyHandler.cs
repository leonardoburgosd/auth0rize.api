using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Command.ApplicationRemoveCompany
{
    internal class ApplicationRemoveCompanyHandler : IRequestHandler<ApplicationRemoveCompany, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationRemoveCompanyHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(ApplicationRemoveCompany request, CancellationToken cancellationToken)
        {
            // Validar que la aplicación existe
            var applicationList = await _unitOfWork.Repository<domain.Application.Application>().QueryAsync<domain.Application.Application>(
                new Dictionary<string, object> { { "Id", request.ApplicationId } },
                Schemas.Organization
            );

            if (!applicationList.Any())
            {
                throw new ApiException("Aplicación no encontrada.");
            }

            // Obtener la relación
            var relationList = await _unitOfWork.Repository<domain.ApplicationCompany.ApplicationCompany>().QueryAsync<domain.ApplicationCompany.ApplicationCompany>(
                new Dictionary<string, object> 
                { 
                    { "ApplicationId", request.ApplicationId },
                    { "CompanyId", request.CompanyId }
                },
                Schemas.Organization
            );

            var relation = relationList.FirstOrDefault();

            if (relation == null)
            {
                throw new ApiException("La relación entre la aplicación y la compañía no existe.");
            }

            // Soft delete de la relación
            await _unitOfWork.Repository<domain.ApplicationCompany.ApplicationCompany>().UpdateByConditionsAsync<domain.ApplicationCompany.ApplicationCompany>(
                conditions: new Dictionary<string, object>
                {
                    { "ApplicationId", request.ApplicationId },
                    { "CompanyId", request.CompanyId }
                },
                values: new Dictionary<string, object>
                {
                    { "IsDeleted", true },
                    { "DateDeleted", DateTime.Now },
                    { "UserDeleted", request.UserDeleted }
                },
                Schemas.Organization
            );

            return new Response<bool>(true, "Compañía desasociada de la aplicación exitosamente.");
        }
    }
}
