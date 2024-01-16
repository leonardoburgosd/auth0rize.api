using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Autentication.Queries.BasicUser
{
    public record BasicUserQuery(string userName, string application) : IRequest<Response<BasicUserResponse>>;
}
