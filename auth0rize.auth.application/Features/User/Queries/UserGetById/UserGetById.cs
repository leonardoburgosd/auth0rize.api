using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.User.Queries.UserGetById
{
    public record UserGetById(int id) : IRequest<Response<UserGetByIdResponse>>;
}
