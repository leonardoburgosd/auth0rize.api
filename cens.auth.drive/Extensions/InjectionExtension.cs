using cens.auth.drive.Intefaces;
using cens.auth.drive.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace cens.auth.drive;
public static class InjectionExtension
{
    public static IServiceCollection AddInjectionDrive(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDriveRepository, DriveRepository>();
        return services;
    }
}
