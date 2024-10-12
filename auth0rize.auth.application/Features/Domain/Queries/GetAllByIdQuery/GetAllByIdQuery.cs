using auth0rize.auth.application.Common.Entities;
using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Domain.Queries.GetAllByIdQuery
{
    public record GetAllByIdQuery(int id, DataSession session) : IRequest<Response<GetAllByIdQueryResponse>>;
}
