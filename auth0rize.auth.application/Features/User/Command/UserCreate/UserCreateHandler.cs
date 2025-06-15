using auth0rize.auth.application.Common.Security;
using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Features.User.Command.FirstAdminCreate;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using auth0rize.auth.domain.User;
using auth0rize.auth.domain.UserDomain;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace auth0rize.auth.application.Features.User.Command.UserCreate
{
    internal class UserCreateHandler : IRequestHandler<UserCreate, Response<UserCreateResponse>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<FirstAdminCreateHandler> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private string? symmetricKey = "";
        private string? url = "";
        public UserCreateHandler(IUnitOfWork unitOfWork, IBackgroundTaskQueue taskQueue, ILogger<FirstAdminCreateHandler> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _taskQueue = taskQueue;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            symmetricKey = Environment.GetEnvironmentVariable(_configuration["security:symmetricKey"]!.ToString());
            url = Environment.GetEnvironmentVariable(_configuration["notification:url"]!.ToString());
        }
        #endregion 

        public async Task<Response<UserCreateResponse>> Handle(UserCreate request, CancellationToken cancellationToken)
        {
            Response<UserCreateResponse> response = new Response<UserCreateResponse>();

            //obtengo el id del superadmin
            var userTypes = await _unitOfWork.Repository<domain.UserType.UserType>().QueryAsync<domain.UserType.UserType>(new Dictionary<string, object>
            {
                { "id", request.typeUserId }
            },
            Schemas.Security);
            if (userTypes.Count() == 0 || userTypes.Count() > 1) throw new ApiException("Tipo de usuario no encontrado.");

            //Obtengo el domain por Id
            var domain = await _unitOfWork.Repository<domain.Domain.Domain>().QueryAsync<domain.Domain.Domain>(new Dictionary<string, object> { { "id", request.domainId } }, Schemas.Security);
            if (domain.Count() == 0) throw new ApiException("Dominio no existe.");

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
                UserRegistration = request.userRegister,
            }, Schemas.Security);

            //registro el usuario y el dominio
            await _unitOfWork.Repository<UserDomain>().InsertNonIdAsync(new UserDomain()
            {
                UserId = idUser,
                DomainId = request.domainId,
                RoleId = userTypes.First().Id,
                UserRegistration = request.userRegister,
            }, Schemas.Security);

            //envío el correo
            _ = _taskQueue.QueueBackgroundWorkItemAsync(async cancellationToken =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var notificationRepository = scope.ServiceProvider.GetRequiredService<IUserNotificationRepository>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    //genero GUID para enviar por correo
                    Guid guidCode = Guid.NewGuid();
                    try
                    {
                        _logger.LogInformation($"Enviando correo de registro en segundo plano a: {request.email}");
                        await notificationRepository.Registration(url + guidCode.ToString(), request.email);
                        _logger.LogInformation($"Correo de registro enviado a: {request.email}");

                        _logger.LogInformation($"Registro de confirmacion: {request.email}");
                        await unitOfWork.Repository<domain.ConfirmAccount.ConfirmAccount>().InsertNonIdAsync(
                            new domain.ConfirmAccount.ConfirmAccount
                            {
                                code = guidCode,
                                UserId = idUser,
                                UserRegistration = idUser,
                                RegistrationDate = DateTime.Now,
                                ExpirationDate = DateTime.Now.AddHours(1),
                            },
                            Schemas.Security
                        );
                        _logger.LogInformation($"Registro de confirmacion realizado");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error al enviar el correo de registro a: {request.email}");
                    }
                }
            });

            response.Success = true;
            response.Message = "Usuario registrado correctamente.";
            response.Data = new UserCreateResponse()
            {
                Id = idUser,
                Email = request.email,
                LastName = request.lastName,
                MotherLastName = request.motherLastName,
                UserName = request.userName,
                Name = request.name,
                DomainId = request.domainId,
                TypeUserId = request.typeUserId,
            };

            return response;
        }
    }
}
