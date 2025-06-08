using auth0rize.auth.domain.Primitives;
using auth0rize.auth.domain.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace auth0rize.auth.notification
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionNotification(this IServiceCollection services, IConfiguration configuration)
        {
            //var emailConfig = configuration.GetSection("notification");
            string? email = Environment.GetEnvironmentVariable(configuration["notification:email"].ToString());
            string? password = Environment.GetEnvironmentVariable(configuration["notification:password"].ToString());
            string? host = Environment.GetEnvironmentVariable(configuration["notification:host"].ToString());
            int? port = Convert.ToInt32(Environment.GetEnvironmentVariable(configuration["notification:port"]));
            if(email is null || password is null || host is null || port is null)
                throw new KeyNotFoundException("Datos de configuración de correo no establecidos.");

            services.AddSingleton<IUserNotificationRepository>(sp => new UserNotificationRepository(
                email,
                password,
                host,
                port!.Value
                ));

            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            services.AddHostedService<QueuedHostedService>();

            return services;
        }
    }
}
