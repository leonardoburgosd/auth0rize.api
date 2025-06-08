using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.User.Command.FirstAdminCreate
{
    public record FirstAdminCreate(
        string name,
        string motherLastName,
        string lastName,
        string userName,
        string email,
        string password
        ) : IRequest<Response<FirstAdminCreateResponse>>;
}
