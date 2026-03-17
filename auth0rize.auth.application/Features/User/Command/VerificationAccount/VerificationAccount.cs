using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.User.Command.VerificationAccount
{
    public record VerificationAccount(string username, string token) : IRequest<Response<bool>>;
}
