using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Command.ApplicationAddCompany
{
    internal class ApplicationAddCompanyHandler : IRequestHandler<ApplicationAddCompany, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationAddCompanyHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(ApplicationAddCompany request, CancellationToken cancellationToken)
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

            // Validar que la compañía existe
            var companyList = await _unitOfWork.Repository<domain.Company.Company>().QueryAsync<domain.Company.Company>(
                new Dictionary<string, object> { { "Id", request.CompanyId } },
                Schemas.Organization
            );

            if (!companyList.Any())
            {
                throw new ApiException("Compañía no encontrada.");
            }

            // Validar que la relación no existe ya
            var existingRelation = await _unitOfWork.Repository<domain.ApplicationCompany.ApplicationCompany>().QueryAsync<domain.ApplicationCompany.ApplicationCompany>(
                new Dictionary<string, object> 
                { 
                    { "ApplicationId", request.ApplicationId },
                    { "CompanyId", request.CompanyId }
                },
                Schemas.Organization
            );

            if (existingRelation.Any())
            {
                throw new ApiException("La compañía ya está asociada a esta aplicación.");
            }

            // Crear la relación
            await _unitOfWork.Repository<domain.ApplicationCompany.ApplicationCompany>().InsertNonIdAsync(
                new domain.ApplicationCompany.ApplicationCompany
                {
                    ApplicationId = request.ApplicationId,
                    CompanyId = request.CompanyId,
                    UserRegistration = request.UserRegistration,
                    RegistrationDate = DateTime.Now
                },
                Schemas.Organization
            );

            return new Response<bool>(true, "Compañía asociada a la aplicación exitosamente.");
        }
    }
}
