using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Queries.ApplicationGetCompanies
{
    public record ApplicationGetCompanies(
        int ApplicationId,
        int Page = 1,
        int Size = 10
    ) : IRequest<Response<ApplicationGetCompaniesResponse>>;
}
