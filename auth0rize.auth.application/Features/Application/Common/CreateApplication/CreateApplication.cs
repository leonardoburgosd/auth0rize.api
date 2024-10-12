using auth0rize.auth.application.Common.Entities;
using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Common.CreateApplication
{
    public record CreateApplication(string name, string description, DataSession session) : IRequest<Response<CreateApplicationResponse>>;
}
