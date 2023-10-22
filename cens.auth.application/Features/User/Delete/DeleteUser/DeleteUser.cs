using cens.auth.application.Common.Entities;
using cens.auth.application.Wrappers;
using MediatR;

namespace cens.auth.application.Features.User.Delete.DeleteUser
{
    public record DeleteUser(int userId, SecurityTokenData SecurityTokenData) : IRequest<Response<bool>>;
}