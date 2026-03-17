using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Company.Command.CompanyUpdate
{
    public record CompanyUpdate(
        int Id,
        string Name,
        int DomainId,
        string? Avatar,
        int UserUpdate
    ) : IRequest<Response<bool>>;
}
