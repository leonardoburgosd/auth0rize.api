using auth0rize.auth.application.Common.Entities;
using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Domain.Command.DeleteDomain
{
    public record DeleteDomain(int id, DataSession session) : IRequest<Response<bool>>;
}
