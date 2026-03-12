using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.User.Queries.UserInfoGet
{
    public record UserInfoGet(int UserId) : IRequest<Response<UserInfoGetResponse>>;
}
