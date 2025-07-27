using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using auth0rize.auth.domain.User;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace auth0rize.auth.application.Features.Autentication.Command.ConfirmAccount
{
    internal class ConfirmAccountHandler : IRequestHandler<ConfirmAccount, Response<bool>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<ConfirmAccountHandler> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private string? url = "";
        public ConfirmAccountHandler(IUnitOfWork unitOfWork, IBackgroundTaskQueue taskQueue, ILogger<ConfirmAccountHandler> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _taskQueue = taskQueue;
            _logger = logger;
            _serviceProvider = serviceProvider;
            url = Environment.GetEnvironmentVariable(_configuration["notification:url"]!.ToString());
        }
        #endregion

        public async Task<Response<bool>> Handle(ConfirmAccount request, CancellationToken cancellationToken)
        {
            Response<bool> response = new Response<bool>();

            var confirmAccount = await _unitOfWork.Repository<domain.ConfirmAccount.ConfirmAccount>().QueryAsync<domain.ConfirmAccount.ConfirmAccount>(new Dictionary<string, object>
            {
                { "code", new Guid(request.code) }
            },
           "security");

            if (confirmAccount.Count() == 0) throw new ApiException("Confirm account no registrado.");

            var confirmAccountUpdate = confirmAccount.First();
            var users = await _unitOfWork.Repository<domain.User.User>().QueryAsync<domain.User.User>(new Dictionary<string, object> { { "Id", confirmAccountUpdate.UserId } }, schema: "security");

            if (confirmAccountUpdate.ExpirationDate < DateTime.Now && !confirmAccountUpdate.IsConfirm)
            {
                //Si es el primer usuario, elimina todos los registros de autenticacion y empieza nuevamente el registro
                if (users.Count() == 1)
                {
                    await DeleteFirstUser(users.First());
                }

                throw new ApiException("El código de confiramción venció.");
            }

            confirmAccountUpdate.IsConfirm = true;
            confirmAccountUpdate.DateUpdate = DateTime.Now;

            await _unitOfWork.Repository<domain.ConfirmAccount.ConfirmAccount>().UpdateAsync(confirmAccountUpdate, "security");

            _ = _taskQueue.QueueBackgroundWorkItemAsync(async cancellationToken =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var notificationRepository = scope.ServiceProvider.GetRequiredService<IUserNotificationRepository>();
                    try
                    {
                        _logger.LogInformation($"Enviando correo de registro en segundo plano a: ");
                        await notificationRepository.RegistrationConfirm(url, users.First().Email);
                        _logger.LogInformation($"Registro de confirmacion realizado");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error al enviar el correo. ");
                    }
                }
            });

            response.Message = "Cuenta confirmada correctamente.";
            response.Success = true;
            return response;
        }

        public async Task DeleteFirstUser(domain.User.User user)
        {
            _ = _taskQueue.QueueBackgroundWorkItemAsync(async cancellationToken =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var notificationRepository = scope.ServiceProvider.GetRequiredService<IUserNotificationRepository>();

                    _logger.LogInformation($"Enviando email registro erroneo");
                    await notificationRepository.RegistrationError(url, user.Email);
                    _logger.LogInformation($"Email registro erroneo enviado");

                    _logger.LogInformation($"Elimina en la tabla confirmacount");
                    await unitOfWork.Repository<domain.ConfirmAccount.ConfirmAccount>().DeleteHardAsync<domain.ConfirmAccount.ConfirmAccount>(new Dictionary<string, object>
                                                                                                        {
                                                                                                            { "UserId", user.Id },
                                                                                                        }, "security");
                    _logger.LogInformation($"Confirmacount eliminado");

                    _logger.LogInformation($"Elimina en la tabla userdomain");
                    await unitOfWork.Repository<domain.UserDomain.UserDomain>().DeleteHardAsync<domain.UserDomain.UserDomain>(new Dictionary<string, object>
                                                                                                        {
                                                                                                            { "UserId", user.Id },
                                                                                                        }, "security");
                    _logger.LogInformation($"Userdomain eliminado");


                    _logger.LogInformation($"Elimina en la tabla domain");
                    await unitOfWork.Repository<domain.Domain.Domain>().DeleteHardAsync<domain.Domain.Domain>(new Dictionary<string, object>
                                                                                                        {
                                                                                                            { "UserRegistration", user.Id },
                                                                                                        }, "security");
                    _logger.LogInformation($"Domain eliminado");

                    _logger.LogInformation($"Elimina en la tabla user");
                    await unitOfWork.Repository<domain.User.User>().DeleteHardAsync<domain.User.User>(new Dictionary<string, object>
                                                                                                        {
                                                                                                            { "Id", user.Id },
                                                                                                        }, "security");
                    _logger.LogInformation($"User eliminado");
                }
            });
        }
    }
}
