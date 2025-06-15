using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.User.Command.UserCreate
{
    public record UserCreate(
        string name,
        string motherLastName,
        string lastName,
        string userName,
        string email,
        string password,
        int typeUserId,
        int domainId,
        int userRegister
        ) :IRequest<Response<UserCreateResponse>>;
}
