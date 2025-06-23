using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Autentication.Queries.Login
{
    public record LoginQuery(string email, string password) : IRequest<Response<LoginResponse>>;
}
