using auth0rize.auth.application.Common.Security;
using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using auth0rize.auth.domain.User;
using auth0rize.auth.domain.UserDomain;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace auth0rize.auth.application.Features.User.Command.FirstAdminCreate
{
    internal class FirstAdminCreateHandler : IRequestHandler<FirstAdminCreate, Response<FirstAdminCreateResponse>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<FirstAdminCreateHandler> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        string? symmetricKey = "";
        public FirstAdminCreateHandler(IUnitOfWork unitOfWork, IBackgroundTaskQueue taskQueue, ILogger<FirstAdminCreateHandler> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _taskQueue = taskQueue;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            symmetricKey = Environment.GetEnvironmentVariable(_configuration["security:symmetricKey"]!.ToString());
        }
        #endregion 

        public async Task<Response<FirstAdminCreateResponse>> Handle(FirstAdminCreate request, CancellationToken cancellationToken)
        {
            Response<FirstAdminCreateResponse> response = new Response<FirstAdminCreateResponse>();

            //obtengo el id del superadmin
            var userTypes = await _unitOfWork.Repository<domain.UserType.UserType>().QueryAsync<domain.UserType.UserType>(new Dictionary<string, object>
            {
                { "name", "superadmin" }
            },
            "security");
            if (userTypes.Count() == 0 || userTypes.Count() > 1) throw new ApiException("Tipo de usuario no encontrado.");

            //verifico si existen superadmins registrados
            var users = await _unitOfWork.Repository<domain.User.User>().QueryAsync<domain.User.User>(new Dictionary<string, object> { { "TypeId", 2 } }, "security");
            if (users.Count() > 0) throw new ApiException("Administrador registrado.");

            //creo al usuario
            (byte[] salt, byte[] password) generate = Encrypt.generateHash(request.password);
            int idUser = await _unitOfWork.Repository<domain.User.User>().InsertAsync(new domain.User.User()
            {
                FirstName = request.name,
                LastName = request.lastName,
                MotherLastName = request.motherLastName,
                UserName = request.userName,
                Email = request.email,

                PasswordHash = generate.password,
                PasswordSalt = generate.salt,
                
                TypeId = userTypes.First().Id,
                UserRegistration = 0
            }, "security");

            //creo el dominio
            int idDomain = await _unitOfWork.Repository<domain.Domain.Domain>().InsertAsync(new domain.Domain.Domain()
            {
                UserRegistration = idUser
            }, "security");

            //registro el usuario y el dominio
            await _unitOfWork.Repository<domain.UserDomain.UserDomain>().InsertAsync(new UserDomain()
            {
                UserId = idUser,
                DomainId = idDomain,
                RoleId = userTypes.First().Id,
                UserRegistration = idUser
            }, "security");

            //genero un token para enviar por correo
            JwtSecurityToken tokenGenerated = Encrypt.generateTokenVerification(new TokenParametersVerification()
            {
                Id = idUser,
                Name = request.name,
                LastName = request.lastName,
                MotherLastName= request.motherLastName,
                UserName = request.userName,
                Email = request.email,
                Role = "superadmin",
                SecretKey = symmetricKey,
            });
            string tokenValidation = new JwtSecurityTokenHandler().WriteToken(tokenGenerated);


            //envío el correo
            _ = _taskQueue.QueueBackgroundWorkItemAsync(async cancellationToken =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var notificationRepository = scope.ServiceProvider.GetRequiredService<IUserNotificationRepository>();
                    try
                    {
                        _logger.LogInformation($"Enviando correo de registro en segundo plano a: {request.email}");
                        await notificationRepository.Registration(tokenValidation, request.email);
                        _logger.LogInformation($"Correo de registro enviado a: {request.email}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error al enviar el correo de registro a: {request.email}");
                    }
                }
            });

            response.Success = true;
            response.Message = "Usuario registrado correctamente.";
            response.Data = new FirstAdminCreateResponse()
            {
                Id = idUser,
                Email = request.email,
                LastName = request.lastName,
                MotherLastName = request.motherLastName,
                UserName = request.userName,
                Name = request.name
            };

            return response;
        }
    }
}
