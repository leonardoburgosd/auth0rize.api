using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.User.Command.UserUpdate
{
    public record UserUpdate(
        int UserId,
        string FirstName,
        string LastName,
        string MotherLastName,
        string UserName,
        string Email,
        int TypeId
    ) : IRequest<Response<bool>>;
}
