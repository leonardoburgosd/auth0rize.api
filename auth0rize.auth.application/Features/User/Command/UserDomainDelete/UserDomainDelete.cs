using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.User.Command.UserDomainDelete
{
    public record UserDomainDelete(
        int UserId,
        string DomainCode,
        int UserDeleted
    ) : IRequest<Response<bool>>;
}
