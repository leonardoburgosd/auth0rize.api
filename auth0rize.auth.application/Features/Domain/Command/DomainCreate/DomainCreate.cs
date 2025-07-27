using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Domain.Command.DomainCreate
{
    public record DomainCreate(int userId):IRequest<Response<DomainCreateResponse>>;
}
