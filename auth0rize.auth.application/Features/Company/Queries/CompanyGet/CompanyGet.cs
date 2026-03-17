using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Company.Queries.CompanyGet
{
    public record CompanyGet(
        string? Search,
        int? DomainId,
        int Page = 1,
        int Size = 10
    ) : IRequest<Response<CompanyGetResponse>>;
}
