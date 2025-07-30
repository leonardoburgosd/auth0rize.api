using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Features.Autentication.Queries.Login;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Login;
using auth0rize.auth.domain.Primitives;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace auth0rize.auth.application.Features.User.Queries.UserVerification
{
    internal class UserVerificationHandler : IRequestHandler<UserVerification, Response<UserVerificationResponse>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<LoginQueryHandler> _logger;
        public UserVerificationHandler(IUnitOfWork unitOfWork, IBackgroundTaskQueue taskQueue, IServiceProvider serviceProvider, ILogger<LoginQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _taskQueue = taskQueue;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        #endregion

        public async Task<Response<UserVerificationResponse>> Handle(UserVerification request, CancellationToken cancellationToken)
        {
            Response<UserVerificationResponse> response = new Response<UserVerificationResponse>();

            var users = await _unitOfWork.Repository<domain.User.User>().QueryAsync<domain.User.User>(new Dictionary<string, object> { { "Email", request.userName } }, Schemas.Security);

            if (users.Count() == 0)
            {
                EnqueueUsernameNonExist(request.userName);
                throw new ApiException("Nombre de usuario o correo no encontrados.");
            }

            EnqueueUsernameExist(request.userName);

            response.Success = true;
            response.Data = new UserVerificationResponse()
            {
                Email = users.First().Email,
                UserName = users.First().UserName,
            };
            response.Message = "Usuario verificado correctamente.";

            return response;
        }

        private void EnqueueUsernameNonExist(string username)
        {
            _taskQueue.QueueBackgroundWorkItemAsync(async token =>
            {
                using var scope = _serviceProvider.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<LoginQueryHandler>>();

                try
                {
                    _logger.LogInformation($"Registra la verificacion del username: {username}");
                    domain.Login.Login login = new domain.Login.Login() {
                        Checked = false,
                        Description = $"Verificacion incorrecta de usuario: {username}",
                        Type = LoginHistoryType.VerificacionUser,
                        UserName = username,
                    };
                    await unitOfWork.Repository<domain.Login.Login>().InsertNonIdAsync(login, Schemas.History);
                    _logger.LogInformation($"Enviando correo de registro en segundo plano a: {username}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error al verificar el username: {username}");
                }
            });
        }

        private void EnqueueUsernameExist(string username)
        {
            _taskQueue.QueueBackgroundWorkItemAsync(async cancellationToken =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    try
                    {
                        _logger.LogInformation($"Registra la verificacion del username: {username}");
                        domain.Login.Login login = new domain.Login.Login()
                        {
                            Checked = true,
                            Description = $"Verificacion correcta de usuario: {username}",
                            Type = LoginHistoryType.VerificacionUser,
                            UserName = username,
                        };
                        await unitOfWork.Repository<domain.Login.Login>().InsertNonIdAsync(login, Schemas.History);
                        _logger.LogInformation($"Enviando correo de registro en segundo plano a: {username}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error al verificar el username: {username}");
                    }
                }
            });
        }
    }
}
