using System.IdentityModel.Tokens.Jwt;
using cens.auth.application.Common.Security;
using cens.auth.application.Wrappers;
using cens.auth.domain.Primitives;
using cens.auth.domain.User.Business;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace cens.auth.application.Features.Authentication.Queries.Login
{

    internal sealed class LoginQueryHandler : IRequestHandler<LoginQuery, Response<LoginResponse>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public LoginQueryHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        #endregion

        public async Task<Response<LoginResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            Response<LoginResponse> response = new Response<LoginResponse>();

            UserDetail? user = await _unitOfWork.User.get(request.UserName, request.Key);
            if (user is null)
                throw new KeyNotFoundException("Usuario o password incorrectos.");

            if (!Encrypt.compareHash(request.Password, user.Password, user.Salt))
                throw new KeyNotFoundException("Usuario o password incorrectos.");

            JwtSecurityToken token = Encrypt.generateTokenValidation(new TokenParameters()
            {
                Id = user.Id,
                UserName = user.UserName,
                Name = $"{user.Name} {user.LastName} {user.MotherLastName}",
                Email = user.Email,
                Role = user.Detail,
                Key = request.Key,
                MultipleFactor = user.IsDoubleFactorActivate,
                Issuer = _configuration["security:issuer"]!.ToString(),
                Audience = _configuration["security:audience"]!.ToString(),
                HoursExpires = Convert.ToInt32(_configuration["security:hourExpiring"]),
                SecretKey = _configuration["security:symmetricKey"]!.ToString(),
                Avatar = user.Avatar,
                Drive = "",
            });

            //Actualiza el token en la tabla

            response.Success = true;
            response.Message = "Usuario autenticado correctamente.";
            response.Data = new LoginResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UsuarioNombre = $"{user.Name} {user.LastName} {user.MotherLastName}",
            };

            return response;
        }
    }
}