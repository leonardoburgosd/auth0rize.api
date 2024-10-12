using auth0rize.auth.application.Common.Entities;
using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Domain.Queries.GetAllQuery
{
    public record GetAllQuery(DataSession session) : IRequest<Response<List<GetAllQueryResponse>>>;
}
