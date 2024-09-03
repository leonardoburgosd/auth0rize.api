using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Email.Common.SendEmail
{
    public record SendEmailCommon(string email, string text) : IRequest<Response<SendEmailResponse>>;
}
