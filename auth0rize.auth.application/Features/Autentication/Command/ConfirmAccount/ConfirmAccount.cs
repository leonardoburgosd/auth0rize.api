using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Autentication.Command.ConfirmAccount
{
    public record ConfirmAccount(string code) : IRequest<Response<bool>>;
}
