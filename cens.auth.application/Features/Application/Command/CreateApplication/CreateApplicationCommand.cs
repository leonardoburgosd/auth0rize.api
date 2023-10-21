using cens.auth.application.Common.Entities;
using cens.auth.application.Wrappers;
using MediatR;

namespace cens.auth.application.Features.Application.Command.CreateApplication
{
    public record CreateApplicationCommand(string name, string Icon,
                                    SecurityTokenData SecurityTokenData) : IRequest<Response<bool>>;

}