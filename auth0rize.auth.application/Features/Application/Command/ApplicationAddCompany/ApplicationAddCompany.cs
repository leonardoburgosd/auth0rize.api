using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Command.ApplicationAddCompany
{
    public record ApplicationAddCompany(
        int ApplicationId,
        int CompanyId,
        int UserRegistration
    ) : IRequest<Response<bool>>;
}
