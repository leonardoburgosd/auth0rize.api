using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Domain.Command.UserDomainCreate
{
    public record UserDomainCreate(
        int UserId,
        string DomainCode,
        int RoleId,
        int UserRegistration
    ) : IRequest<Response<bool>>;
}
