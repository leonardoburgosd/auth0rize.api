using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Company.Command.CompanyDelete
{
    public record CompanyDelete(
        int Id,
        int UserDeleted
    ) : IRequest<Response<bool>>;
}
