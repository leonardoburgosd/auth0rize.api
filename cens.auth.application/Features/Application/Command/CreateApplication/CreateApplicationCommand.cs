using cens.auth.application.Common.Entities;
using cens.auth.application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace cens.auth.application.Features.Application.Command.CreateApplication
{
    public record CreateApplicationCommand(string name, IFormFile Icon, string Description,
                                    SecurityTokenData SecurityTokenData) : IRequest<Response<bool>>;

}