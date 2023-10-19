using cens.auth.application.Wrappers;
using MediatR;

namespace cens.auth.application.Features.Authentication.Queries.Login
{
    public record LoginQuery(string UserName, string Password, string Key) : IRequest<Response<LoginResponse>>;
}
