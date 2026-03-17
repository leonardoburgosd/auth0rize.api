using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Command.ApplicationUpdate
{
    public record ApplicationUpdate(
        int Id,
        string Name,
        string Description,
        string Avatar,
        int UserUpdate
    ) : IRequest<Response<bool>>;
}
