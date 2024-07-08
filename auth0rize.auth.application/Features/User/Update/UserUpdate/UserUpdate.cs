using auth0rize.auth.application.Common.Entities;
using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.User.Update.UserUpdate
{
    public record UserUpdate(
        long id,
        string name,
        string lastName,
        string motherLastName,
        string email,
        DataSession session,
        long type) : IRequest<Response<bool>>;
}
