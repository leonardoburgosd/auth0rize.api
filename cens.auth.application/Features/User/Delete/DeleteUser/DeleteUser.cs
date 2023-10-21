using cens.auth.application.Wrappers;
using MediatR;

namespace cens.auth.application.Features.User.Delete.DeleteUser
{
    public record DeleteUser(int userId): IRequest<Response<bool>>;
}