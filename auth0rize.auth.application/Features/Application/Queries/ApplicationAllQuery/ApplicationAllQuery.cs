using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Queries.ApplicationAllQuery
{
    public record ApplicationAllQuery(int userId) : IRequest<Response<List<ApplicationAllQueryResponse>>>;
}
