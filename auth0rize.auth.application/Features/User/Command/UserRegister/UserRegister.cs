using auth0rize.auth.application.Common.Entities;
using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.User.Command.UserRegister
{
    public record UserRegister(string name, string lastName, string motherLastName, string userName, string email, string password, DataSession session, long domain = 0, long type = 1) : IRequest<Response<UserRegisterResponse>>;
}
