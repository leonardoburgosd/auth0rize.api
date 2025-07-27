using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Domain.Queries.DomainGet
{
    public record DomainGet(string? search, string? state, int page = 1, int size = 10) : IRequest<Response<DomainGetResponse>>;     
}
