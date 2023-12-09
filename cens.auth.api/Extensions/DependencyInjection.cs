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

            string? urlPermissions = Environment.GetEnvironmentVariable(configuration["urlPermission:consultingPolicy"]!.ToString());
            string? urlAuthentication = Environment.GetEnvironmentVariable(configuration["urlPermission:authenticationPolicy"]!.ToString());
            string[] permissionsArray = urlPermissions!.Split(',');
            string[] authenticationArray = urlAuthentication!.Split(',');

            services.AddCors(p =>
            {
                p.AddPolicy("consultingPolicy", app =>
                {
                    app.WithOrigins(permissionsArray)
                       .WithMethods("POST", "GET", "DELETE", "PUT")
                       .WithHeaders("Authorization");
                });

                p.AddPolicy("authenticationPolicy", policy =>
                {
                    policy.WithOrigins(authenticationArray).WithMethods("POST").AllowAnyHeader();
                });

            });

            string? symmetricKey = Environment.GetEnvironmentVariable(configuration["security:symmetricKey"]!.ToString());
            string? issuer = Environment.GetEnvironmentVariable(configuration["security:issuer"]!.ToString());
            string? audience = Environment.GetEnvironmentVariable(configuration["security:audience"]!.ToString());
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                    AddJwtBearer(options =>
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmetricKey)),
                        ClockSkew = TimeSpan.Zero
                    });

            services.AddControllers();
            services.AddApiVersioningExtension();
            services.AddEndpointsApiExplorer();

            return services;
        }
    }
}
