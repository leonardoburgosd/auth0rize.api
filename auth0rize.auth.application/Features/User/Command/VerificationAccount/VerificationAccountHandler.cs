using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.User.Command.VerificationAccount
{
    internal sealed class VerificationAccountHandler : IRequestHandler<VerificationAccount, Response<bool>>
    {
        public async Task<Response<bool>> Handle(VerificationAccount request, CancellationToken cancellationToken)
        {
            Response<bool> response = new Response<bool>();

            response.Success = true;
            response.Data = true;
            response.Message = "Cuenta verificada correctamente.";

            return response;
        }
    }
}
