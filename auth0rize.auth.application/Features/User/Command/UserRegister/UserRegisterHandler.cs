using auth0rize.auth.application.Common.Security;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Domain.Business;
using auth0rize.auth.domain.Primitives;
using auth0rize.auth.domain.TypeUser;
using auth0rize.auth.domain.User.Business;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace auth0rize.auth.application.Features.User.Command.UserRegister
{
    internal sealed class UserRegisterHandler : IRequestHandler<UserRegister, Response<UserRegisterResponse>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private string? issuer = "";
        private string? audience = "";
        private string? symmetricKey = "";
        private int hours = 0;
        public UserRegisterHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            issuer = Environment.GetEnvironmentVariable(_configuration["security:issuer"]!.ToString());
            audience = Environment.GetEnvironmentVariable(_configuration["security:audience"]!.ToString());
            hours = Convert.ToInt32(Environment.GetEnvironmentVariable(_configuration["security:hourExpiring"]));
            symmetricKey = Environment.GetEnvironmentVariable(_configuration["security:symmetricKey"]!.ToString());
        }
        #endregion 

        public async Task<Response<UserRegisterResponse>> Handle(UserRegister request, CancellationToken cancellationToken)
        {
            Response<UserRegisterResponse> response = new Response<UserRegisterResponse>();

            if (await _unitOfWork.User.userNameExist(request.userName))
                throw new KeyNotFoundException("Nombre de usuario ya existe.");

            if (await _unitOfWork.User.emailExist(request.email))
                throw new KeyNotFoundException("Correo electrónico ya existe.");

            TypeUser? type = await _unitOfWork.TypeUser.get(request.type);
            if (type is null) throw new KeyNotFoundException("Tipo de usuario no existe.");

            (byte[] salt, byte[] password) generate = Encrypt.generateHash(request.password);
            bool exist = true;
            string domainCode = "";
            while (exist)
            {
                domainCode = generateDomainCode();
                exist = await _unitOfWork.Domain.exist(domainCode);
            }

            long? domainId = await _unitOfWork.Domain.create(new DomainCreate() { Name = domainCode, UserRegistration = 1 });

            if (domainId is null || domainId == 0) throw new KeyNotFoundException("Error al generar el dominio. Intente más tarde.");

            long? userId = await _unitOfWork.User.create(new UserCreate()
            {
                Name = request.name,
                LastName = request.lastName,
                MotherLastName = request.motherLastName,
                Username = request.userName,
                Email = request.email,
                Password = generate.password,
                Salt = generate.salt,
                Avatar = "default.png",
                Type = request.type,
                Domain = request.session is null ? domainId ?? 0 : request.session.Domain,
                UserRegistration = request.session is null ? 0 : request.session.Id,
            });

            if (userId is null || userId == 0) throw new KeyNotFoundException("Usuario no se pudo crear correctamente.");

            string token = "";
            if (request.session is null) token = Encrypt.generateTokenValidation(new TokenParameters()
            {
                Id = (long)userId,
                UserName = request.userName,
                Name = request.name,
                MotherLastName = request.motherLastName,
                LastName = request.lastName,
                Email = request.email,
                Role = type.Name,
                MultipleFactor = true,
                Issuer = issuer,
                Audience = audience,
                HoursExpires = hours,
                SecretKey = symmetricKey,
                Avatar = "default.png",
                Domain = domainCode
            }).ToString();

            response.Success = true;
            response.Message = "Usuario creado correctamente.";
            response.Data = new UserRegisterResponse() { Token = token };

            return response;
        }

        private string generateDomainCode()
        {
            int length = 25;
            Random random = new Random();
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789;[{}]:,.><_-+=(*&^%$#@!|)";
            var result = new StringBuilder(length);
            var timestamp = DateTime.UtcNow.Ticks.ToString("x");
            result.Append(timestamp);
            for (int i = timestamp.Length; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }
    }
}
