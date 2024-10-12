using auth0rize.auth.application.Common.Entities;
using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Common.DeletedApplication
{
    public record DeletedApplication(int idApplication, DataSession session) : IRequest<Response<bool>>;
}
