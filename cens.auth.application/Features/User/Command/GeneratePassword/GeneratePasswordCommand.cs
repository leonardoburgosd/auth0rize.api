using cens.auth.application.Wrappers;
using MediatR;

namespace cens.auth.application.Features.User.Command.GeneratePassword
{
    public record GeneratePasswordCommand() : IRequest<Response<GeneratePasswordCommandResponse>>;
        
}