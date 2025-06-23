using auth0rize.auth.application.Common.Security;
using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Domain;
using auth0rize.auth.domain.Primitives;
using auth0rize.auth.domain.User;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace auth0rize.auth.application.Features.Autentication.Queries.Login
{
    internal sealed class LoginQueryHandler : IRequestHandler<LoginQuery, Response<LoginResponse>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<LoginQueryHandler> _logger;
        private readonly IServiceProvider _serviceProvider;
        private string? issuer = "";
        private string? symmetricKey = "";
        private int hours = 0;
        string? audience = "";
        public LoginQueryHandler(IUnitOfWork unitOfWork, IConfiguration configuration, IBackgroundTaskQueue taskQueue, ILogger<LoginQueryHandler> logger, IServiceProvider serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            issuer = Environment.GetEnvironmentVariable(_configuration["security:issuer"]!.ToString());
            audience = Environment.GetEnvironmentVariable(_configuration["security:audience"]!.ToString());
            symmetricKey = Environment.GetEnvironmentVariable(_configuration["security:symmetricKey"]!.ToString());
            hours = Convert.ToInt32(Environment.GetEnvironmentVariable(_configuration["security:hourExpiring"]));
            _taskQueue = taskQueue;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        #endregion 

        public async Task<Response<LoginResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            Response<LoginResponse> response = new Response<LoginResponse>();

            var users = await _unitOfWork.Repository<domain.User.User>().QueryWithRelationsAsync<domain.User.User>(
                    entitySql: "SELECT * FROM security.user WHERE isDeleted = false and email = @Email",
                    parameters: new { Email = request.email },
                    relationsPaths: new string[] { "UsersDomains" }
            );

            if (users.Count() == 0) throw new ApiException("Correo o contraseña incorrecto(s).");
            domain.User.User user = users.First();

            if (!Encrypt.compareHash(request.password, user.PasswordHash, user.PasswordSalt))
                throw new KeyNotFoundException("Correo o contraseña incorrecto(s).");

            Domain domain = await getDomainByUser(user.UsersDomains.First().DomainId);
            domain.UserType.UserType type = await getUser(user.TypeId);

            JwtSecurityToken tokenGenerated = Encrypt.generateTokenValidation(new TokenParameters()
            {
                Id = user.Id,
                UserName = user.UserName,
                Name = user.FirstName,
                MotherLastName = user.MotherLastName,
                LastName = user.LastName,
                Email = user.Email,
                Role = type.Name,
                MultipleFactor = user.IsDoubleFactorActive,
                Issuer = issuer,
                Audience = audience,
                HoursExpires = hours,
                SecretKey = symmetricKey,
                Avatar = user.Avatar,
                Domain = domain.Code.ToString()
            });

            string token = new JwtSecurityTokenHandler().WriteToken(tokenGenerated);

            _ = _taskQueue.QueueBackgroundWorkItemAsync(async cancellationToken =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var notificationRepository = scope.ServiceProvider.GetRequiredService<IUserNotificationRepository>();

                    try
                    {
                        _logger.LogInformation($"Enviando correo de registro en segundo plano a: {request.email}");
                        await notificationRepository.LoginCorrect(request.email);
                        _logger.LogInformation($"Enviando correo de registro en segundo plano a: {request.email}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error al enviar el correo de registro a: {request.email}");
                    }
                }
            });

            response.Success = true;
            response.Message = "Usuario autenticado correctamente.";
            response.Data = new LoginResponse()
            {
                Token = token,
                Email = user.Email,
                Name = $"{user.FirstName} {user.LastName} {user.MotherLastName}",
            };

            return response;
        }

        private async Task<Domain> getDomainByUser(int domainId)
        {
            var domain = await _unitOfWork.Repository<domain.Domain.Domain>().QueryAsync<domain.Domain.Domain>(new Dictionary<string, object> { { "Id", domainId } }, schema: Schemas.Security);
            return domain.First();
        }

        private async Task<domain.UserType.UserType> getUser(int typeId)
        {
            var values = new[] { "admin", "superadmin" };
            var userType = new List<domain.UserType.UserType>();

            foreach (var name in values)
            {
                var result = await _unitOfWork.Repository<domain.UserType.UserType>().QueryAsync<domain.UserType.UserType>(new Dictionary<string, object> { { "name", name } }, Schemas.Security);
                userType.AddRange(result);
            }

            if (userType.Count() == 0) throw new ApiException("Tipos de usuario no encontrados");

            if (userType.Where(ut => ut.Id == typeId).Count() == 0) throw new ApiException("No tiene permisos suficientes para ingresar a este panel.");

            return userType.Where(ut => ut.Id == typeId).First();
        }
    }
}
