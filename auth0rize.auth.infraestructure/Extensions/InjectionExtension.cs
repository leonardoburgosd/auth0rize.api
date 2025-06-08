using auth0rize.auth.domain.Primitives;
using auth0rize.auth.infraestructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;

namespace auth0rize.auth.infraestructure.Extensions
{
    public static class InjectionExtension
    {
        public static IServiceCollection AddInjectionInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            string? connectionString = Environment.GetEnvironmentVariable(configuration["connection:postgres"]!.ToString());
            services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(connectionString));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            return services;
        }
    }

}
