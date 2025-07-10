using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.User.Queries.UserGet
{
    public record UserGet(string? search, string? type, bool? deleted, int page = 1, int size = 10) : IRequest<Response<UserGetResponse>>;
}
