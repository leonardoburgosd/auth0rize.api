using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.User.Queries.UserNameVerification
{
    public record UserNameVerification(string userName): IRequest<Response<bool>>;
}
