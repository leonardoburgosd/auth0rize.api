using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Company.Command.CompanyCreate
{
    public record CompanyCreate(
        string Name,
        string DomainId,
        string? Avatar,
        int UserRegistration
    ) : IRequest<Response<CompanyCreateResponse>>;
}
