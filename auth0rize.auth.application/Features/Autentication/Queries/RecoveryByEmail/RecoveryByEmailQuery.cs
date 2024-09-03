using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Autentication.Queries.RecoveryByEmail
{
    public record RecoveryByEmailQuery(string email) : IRequest<Response<bool>>;
}
