using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Company.Command.CompanyUpdate
{
    internal class CompanyUpdateHandler : IRequestHandler<CompanyUpdate, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyUpdateHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(CompanyUpdate request, CancellationToken cancellationToken)
        {
            // Buscar la compañía
            var companyList = await _unitOfWork.Repository<domain.Company.Company>().QueryAsync<domain.Company.Company>(
                new Dictionary<string, object> { { "Id", request.Id } },
                Schemas.Organization
            );

            var company = companyList.FirstOrDefault();

            if (company == null)
            {
                throw new ApiException("Compañía no encontrada.");
            }

            // Validar que el dominio existe
            var domain = await _unitOfWork.Repository<domain.Domain.Domain>().QueryAsync<domain.Domain.Domain>(
                new Dictionary<string, object> { { "Id", request.DomainId } },
                Schemas.Security
            );

            if (!domain.Any())
            {
                throw new ApiException("El dominio especificado no existe.");
            }

            // Actualizar campos
            company.Name = request.Name;
            company.DomainId = request.DomainId;
            company.Avatar = request.Avatar;
            company.DateUpdate = DateTime.Now;
            company.UserUpdate = request.UserUpdate;

            await _unitOfWork.Repository<domain.Company.Company>().UpdateAsync(company, Schemas.Organization);

            return new Response<bool>(true, "Compañía actualizada exitosamente.");
        }
    }
}
