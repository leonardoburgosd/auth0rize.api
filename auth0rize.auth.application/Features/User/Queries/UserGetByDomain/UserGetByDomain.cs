using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.User.Queries.UserGetByDomain
{
    public record UserGetByDomain(string DomainCode, string? search, int page = 1, int size = 10) : IRequest<Response<UserGetByDomainResponse>>;
}
