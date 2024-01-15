using auth0rize.auth.domain.Primitives;
using auth0rize.auth.infraestructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace auth0rize.auth.infraestructure.Extensions
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
