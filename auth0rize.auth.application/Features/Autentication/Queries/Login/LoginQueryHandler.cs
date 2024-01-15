using auth0rize.auth.application.Common.Security;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using auth0rize.auth.domain.User.Business;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace auth0rize.auth.application.Features.Autentication.Queries.Login
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

            UserDetail user = await _unitOfWork.User.get(request.userName);
            if (user is null) throw new KeyNotFoundException("Usuario no existe.");

            if (!string.IsNullOrEmpty(request.application) && !user.Application.Equals(request.application))
                throw new KeyNotFoundException("Usuario no tiene acceso a esta aplicación.");

            if (!Encrypt.compareHash(request.password, user.Password, user.Salt))
                throw new KeyNotFoundException("Usuario o password incorrectos.");

            string token = "";
            int doubleFactorCode = 0;
            if (!user.IsDoubleFactorActivate)
            {
                string? issuer = Environment.GetEnvironmentVariable(_configuration["security:issuer"]!.ToString());
                string? audience = Environment.GetEnvironmentVariable(_configuration["security:audience"]!.ToString());
                int hours = Convert.ToInt32(Environment.GetEnvironmentVariable(_configuration["security:hourExpiring"]));
                string? symmetricKey = Environment.GetEnvironmentVariable(_configuration["security:symmetricKey"]!.ToString());

                JwtSecurityToken tokenGenerated = Encrypt.generateTokenValidation(new TokenParameters()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Name = $"{user.Name} {user.LastName} {user.MotherLastName}",
                    Email = user.Email,
                    Role = user.TypeUserName,
                    Key = request.application,
                    MultipleFactor = user.IsDoubleFactorActivate,
                    Issuer = issuer,
                    Audience = audience,
                    HoursExpires = hours,
                    SecretKey = symmetricKey,
                    Avatar = user.Avatar
                });
                token = new JwtSecurityTokenHandler().WriteToken(tokenGenerated);
            }
            else
                doubleFactorCode = 123;

            response.Success = true;
            response.Message = "Usuario autenticado correctamente.";
            response.Data = new LoginResponse()
            {
                Token = token,
                DoubleFactorCode = doubleFactorCode
            };

            return response;
        }
    }
}
