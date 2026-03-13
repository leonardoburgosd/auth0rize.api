using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Company.Queries.CompanyGetById
{
    internal class CompanyGetByIdHandler : IRequestHandler<CompanyGetById, Response<CompanyGetByIdResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyGetByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<CompanyGetByIdResponse>> Handle(CompanyGetById request, CancellationToken cancellationToken)
        {
            var companyList = await _unitOfWork.Repository<domain.Company.Company>().QueryAsync<domain.Company.Company>(
                new Dictionary<string, object> { { "Id", request.Id } },
                Schemas.Organization
            );

            var company = companyList.FirstOrDefault();

            if (company == null)
            {
                throw new ApiException("Compañía no encontrada.");
            }

            // Obtener el dominio
            var domainList = await _unitOfWork.Repository<domain.Domain.Domain>().QueryAsync<domain.Domain.Domain>(
                new Dictionary<string, object> { { "Id", company.DomainId } },
                Schemas.Security
            );

            var domain = domainList.FirstOrDefault();

            return new Response<CompanyGetByIdResponse>(
                new CompanyGetByIdResponse
                {
                    Id = company.Id,
                    Name = company.Name,
                    DomainId = company.DomainId,
                    DomainCode = domain?.Code.ToString() ?? string.Empty,
                    Avatar = company.Avatar,
                    RegistrationDate = company.RegistrationDate.ToString("dd-MM-yyyy HH:mm:ss"),
                    IsDeleted = company.IsDeleted
                },
                "Compañía obtenida exitosamente."
            );
        }
    }
}
