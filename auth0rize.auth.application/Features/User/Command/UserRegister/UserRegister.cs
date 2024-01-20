using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.User.Command.UserRegister
{
    public record UserRegister() : IRequest<Response<UserRegisterResponse>>;
}
