using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Role.Queries.RoleGet
{
    public record TypeGet():IRequest<Response<List<TypeGetResponse>>>;
}
