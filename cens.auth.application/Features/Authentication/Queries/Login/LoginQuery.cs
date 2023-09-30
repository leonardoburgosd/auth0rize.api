using cens.auth.application.Common.Security;
using cens.auth.application.Wrappers;
using cens.auth.domain.Bussines;
using cens.auth.infraestructure.Persistence.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace cens.auth.application.Features.Authentication.Queries.Login
{
    public class LoginQuery : IRequest<Response<LoginResponse>>
    {
        public string UserName { get; set; } //email or username
        public string Password { get; set; }
        public string Key { get; set; }
    }
    public class LoginQueryHandler : IRequestHandler<LoginQuery, Response<LoginResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public LoginQueryHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<Response<LoginResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            Response<LoginResponse> response = new Response<LoginResponse>();

            UserDetail user = await _unitOfWork.User.get(request.UserName, request.Key);
            if (user is null)
                throw new KeyNotFoundException("Usuario o password incorrectos.");

            if (!Encrypt.compareHash(request.Password, user.Password, user.Salt))
                throw new KeyNotFoundException("Usuario o password incorrectos.");

            string token = Encrypt.generateTokenValidation(new TokenParameters()
            {
                Id = user.Id,
                UserName = user.UserName,
                Name = $"{user.Name} {user.LastName} {user.MotherLastName}",
                Email = user.Email,
                Role = user.Detail,
                Key = request.Key,
                MultipleFactor = user.IsDoubleFactorActivate,
                Issuer = _configuration["security:Issuer"],
                Audience = _configuration["security:Audience"],
                HoursExpires = Convert.ToInt32(_configuration["security:HourExpiring"]),
                SecretKey = _configuration["security:Key"].ToString(),
                Avatar = user.Avatar,
                Drive = "",
            });

            response.Success = true;
            response.Message = "Usuario autenticado correctamente.";
            response.Data = new LoginResponse()
            {
                Token = token,
                UsuarioNombre = $"{user.Name} {user.LastName} {user.MotherLastName}",
            };

            return response;
        }

    }
}
