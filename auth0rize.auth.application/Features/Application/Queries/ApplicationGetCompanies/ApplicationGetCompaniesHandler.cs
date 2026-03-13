using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Queries.ApplicationGetCompanies
{
    internal class ApplicationGetCompaniesHandler : IRequestHandler<ApplicationGetCompanies, Response<ApplicationGetCompaniesResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationGetCompaniesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<ApplicationGetCompaniesResponse>> Handle(ApplicationGetCompanies request, CancellationToken cancellationToken)
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

            // Obtener las compañías asociadas a la aplicación
            var (applicationCompanies, total) = await _unitOfWork.Repository<domain.ApplicationCompany.ApplicationCompany>().QueryPagedAsync<domain.ApplicationCompany.ApplicationCompany>(
                filters: new Dictionary<string, object> { { "ApplicationId", request.ApplicationId } },
                skip: request.Page - 1,
                take: request.Size,
                orderBy: "RegistrationDate",
                ascending: false,
                schema: Schemas.Organization
            );

            var companyIds = applicationCompanies.Select(ac => ac.CompanyId).Distinct().ToList();

            // Obtener los datos de las compañías
            var companies = new List<domain.Company.Company>();
            foreach (var companyId in companyIds)
            {
                var companyList = await _unitOfWork.Repository<domain.Company.Company>().QueryAsync<domain.Company.Company>(
                    new Dictionary<string, object> { { "Id", companyId } },
                    Schemas.Organization
                );
                companies.AddRange(companyList);
            }

            // Obtener los dominios
            var domainIds = companies.Select(c => c.DomainId).Distinct().ToList();
            var domains = new List<domain.Domain.Domain>();
            foreach (var domainId in domainIds)
            {
                var domainList = await _unitOfWork.Repository<domain.Domain.Domain>().QueryAsync<domain.Domain.Domain>(
                    new Dictionary<string, object> { { "Id", domainId } },
                    Schemas.Security
                );
                domains.AddRange(domainList);
            }

            var companiesList = applicationCompanies.Select(ac =>
            {
                var company = companies.FirstOrDefault(c => c.Id == ac.CompanyId);
                var domain = domains.FirstOrDefault(d => d.Id == company?.DomainId);

                return new ApplicationCompanyListResponse
                {
                    CompanyId = ac.CompanyId,
                    CompanyName = company?.Name ?? string.Empty,
                    DomainId = company?.DomainId ?? 0,
                    DomainCode = domain?.Code.ToString() ?? string.Empty,
                    Avatar = company?.Avatar,
                    RegistrationDate = ac.RegistrationDate.ToString("dd-MM-yyyy HH:mm:ss")
                };
            }).ToList();

            return new Response<ApplicationGetCompaniesResponse>(
                new ApplicationGetCompaniesResponse
                {
                    Total = total,
                    Page = request.Page,
                    Size = request.Size,
                    Companies = companiesList
                },
                "Compañías de la aplicación obtenidas exitosamente."
            );
        }
    }
}
