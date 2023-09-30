using cens.auth.infraestructure.Persistence.Interfaces;
using cens.auth.infraestructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace cens.auth.infraestructure.Extensions
{
    public static class InjectionExtension
    {
        public static IServiceCollection AddInjectionInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
