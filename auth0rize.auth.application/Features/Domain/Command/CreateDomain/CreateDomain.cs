using auth0rize.auth.application.Common.Entities;
using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Domain.Command.CreateDomain
{
    public record CreateDomain(string name, DataSession session) : IRequest<Response<CreateDomainResponse>>;
}
