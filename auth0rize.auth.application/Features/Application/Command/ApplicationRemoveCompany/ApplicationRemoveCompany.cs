using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Command.ApplicationRemoveCompany
{
    public record ApplicationRemoveCompany(
        int ApplicationId,
        int CompanyId,
        int UserDeleted
    ) : IRequest<Response<bool>>;
}
