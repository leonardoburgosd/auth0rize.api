using cens.auth.application.Common.Entities;
using cens.auth.application.Wrappers;
using MediatR;

namespace cens.auth.application.Features.User.Command.CreateUser
{

    public record CreateUserCommand(
                                    string Name, string LastName, string MotherLastName, DateTime Birthday,
                                    string UserName, string Password, string Email,
                                    SecurityTokenData SecurityTokenData) : IRequest<Response<bool>>;

}