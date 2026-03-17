using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Company.Queries.CompanyGet
{
    internal class CompanyGetHandler : IRequestHandler<CompanyGet, Response<CompanyGetResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyGetHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<CompanyGetResponse>> Handle(CompanyGet request, CancellationToken cancellationToken)
        {
            var filters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(request.Search))
            {
                filters.Add("Name", request.Search);
            }

            if (request.DomainId.HasValue)
            {
                filters.Add("DomainId", request.DomainId.Value);
            }

            var (data, total) = await _unitOfWork.Repository<domain.Company.Company>().QueryPagedAsync<domain.Company.Company>(
                filters: filters,
                skip: request.Page - 1,
                take: request.Size,
                orderBy: "Name",
                ascending: true,
                schema: Schemas.Organization
            );

            // Obtener los IDs de dominios únicos
            var domainIds = data.Select(c => c.DomainId).Distinct().ToList();
            
            // Consultar los dominios
            var domains = new List<domain.Domain.Domain>();
            foreach (var domainId in domainIds)
            {
                var domainList = await _unitOfWork.Repository<domain.Domain.Domain>().QueryAsync<domain.Domain.Domain>(
                    new Dictionary<string, object> { { "Id", domainId } },
                    Schemas.Security
                );
                domains.AddRange(domainList);
            }

            var companiesList = data.Select(c => new CompanyListResponse
            {
                Id = c.Id,
                Name = c.Name,
                DomainId = c.DomainId,
                DomainCode = domains.FirstOrDefault(d => d.Id == c.DomainId)?.Code.ToString() ?? string.Empty,
                Avatar = c.Avatar,
                RegistrationDate = c.RegistrationDate.ToString("dd-MM-yyyy HH:mm:ss"),
            }).ToList();

            return new Response<CompanyGetResponse>(
                new CompanyGetResponse
                {
                    Total = total,
                    Page = request.Page,
                    Size = request.Size,
                    Companies = companiesList
                },
                "Lista de compañías obtenida exitosamente."
            );
        }
    }
}
