using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.User.Command.VerificationUser
{
    public record VerificationUser() : IRequest<Response<bool>>;
}
