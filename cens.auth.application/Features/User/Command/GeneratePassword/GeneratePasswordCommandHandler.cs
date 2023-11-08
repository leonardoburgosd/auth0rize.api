using cens.auth.application.Wrappers;
using MediatR;
using System.Security.Cryptography;


namespace cens.auth.application.Features.User.Command.GeneratePassword
{
    public class GeneratePasswordCommandHandler : IRequestHandler<GeneratePasswordCommand, Response<GeneratePasswordCommandResponse>>
    {
        public async Task<Response<GeneratePasswordCommandResponse>> Handle(GeneratePasswordCommand request, CancellationToken cancellationToken)
        {
            Response<GeneratePasswordCommandResponse> response = new Response<GeneratePasswordCommandResponse>();

            response.Success = true;
            response.Message = "";
            response.Data = new GeneratePasswordCommandResponse()
            {
                Password = generatePassword()
            };

            return response;
        }

        private string generatePassword()
        {
            int saltByteSize = 16; // Tamaño de la sal (ajusta según tus necesidades)
            int iterations = 10000; // Número de iteraciones (ajusta según tus necesidades)
            int hashByteSize = 32; // Tamaño del hash de salida (ajusta según tus necesidades)

            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[saltByteSize];
                rng.GetBytes(salt);

                using (var pbkdf2 = new Rfc2898DeriveBytes(Guid.NewGuid().ToString(), salt, iterations))
                {
                    byte[] hash = pbkdf2.GetBytes(hashByteSize);
                    return Convert.ToBase64String(hash).Substring(0, 20); // Limitar la longitud
                }
            }
        }
    }
}