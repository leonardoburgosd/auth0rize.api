using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.User.Command.UserUpdateDoubleFactor
{
    public record UserUpdateDoubleFactor(int UserId, bool IsActive) : IRequest<Response<bool>>;
}
