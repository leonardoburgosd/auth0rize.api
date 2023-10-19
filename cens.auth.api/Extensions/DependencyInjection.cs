using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace cens.auth.api.Extensions
{
    public static class DependencyInjection
    {
        private static string[] autorizacionManejoPersonal = { "Superadmin", "Admin", "GestionHumana" };
        public static IServiceCollection AddPresentation(this IServiceCollection services, ConfigurationManager configuration)
        {

            services.AddSwaggerGen();
            // services.AddAuthorization(options =>
            // {
            //     options.AddPolicy("ManejoDePersonal", policy => policy.RequireRole(autorizacionManejoPersonal));
            // });
            // services.AddHttpsRedirection(options =>
            // {
            //     options.HttpsPort = 5001;
            // });
            services.AddCors(p =>
            {
                p.AddPolicy("authpolicy", app => app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["security:issuer"]?.ToString(),
                    ValidAudience = configuration["security:audience"]?.ToString(),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["security:symmetricKey"]!.ToString())),
                    ClockSkew = TimeSpan.Zero
                }
            );
            services.AddControllers();

            services.AddApiVersioningExtension();
            services.AddEndpointsApiExplorer();

            return services;
        }
    }
}
