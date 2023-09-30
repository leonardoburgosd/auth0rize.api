using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace cens.auth.application.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.Configure<FormOptions>(options => options.MultipartBodyLengthLimit = long.MaxValue);
            return services;
        }
    }
}
