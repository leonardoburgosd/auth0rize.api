using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.User.Queries.UserGetCounter
{
    public record UserGetCounter():IRequest<Response<UserGetCounterResponse>>;
}
