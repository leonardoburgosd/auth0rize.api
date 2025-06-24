using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.User.Queries.UserVerification
{
    public record UserVerification(string userName) : IRequest<Response<UserVerificationResponse>>;
}
