using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Command.ApplicationCreate
{
    public record ApplicationCreate(
        string Name,
        string Description,
        string Avatar,
        int UserRegistration
    ) : IRequest<Response<int>>;
}
