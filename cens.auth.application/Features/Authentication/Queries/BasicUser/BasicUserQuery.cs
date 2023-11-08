using cens.auth.application.Wrappers;
using MediatR;

namespace cens.auth.application.Features.Authentication.Queries.BasicUser
{
    public record BasicUserQuery(string UserName, string Key) : IRequest<Response<BasicUserResponse>>;
}
