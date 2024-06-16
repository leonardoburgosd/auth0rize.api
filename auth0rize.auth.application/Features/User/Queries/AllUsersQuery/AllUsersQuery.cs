using auth0rize.auth.application.Common.Entities;
using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.User.Queries.AllUsersQuery
{
    public record AllUsersQuery(DataSession session) : IRequest<Response<List<AllUsersQueryResponse>>>;
}
