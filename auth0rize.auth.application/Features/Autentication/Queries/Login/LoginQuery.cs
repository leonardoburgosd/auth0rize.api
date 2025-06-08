using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Autentication.Queries.Login
{
    public record LoginQuery(string userName, string password) : IRequest<Response<LoginResponse>>;
}
