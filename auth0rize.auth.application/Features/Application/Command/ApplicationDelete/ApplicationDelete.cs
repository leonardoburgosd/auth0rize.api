using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Command.ApplicationDelete
{
    public record ApplicationDelete(int Id, int UserDeleted) : IRequest<Response<bool>>;
}
