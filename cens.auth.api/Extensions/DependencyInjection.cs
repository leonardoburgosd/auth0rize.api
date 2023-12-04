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
                p.AddPolicy("consultingPolicy", app =>
                {
                    app.WithOrigins("https://workforce.censperu.com")
                       .WithMethods("POST", "GET", "DELETE", "PUT");
                });

                p.AddPolicy("authenticationPolicy", policy =>
                {
                    policy.WithOrigins("https://auth.censperu.com").WithMethods("POST");
                });

            });

            //services.AddCors(p =>
            //{
            //    p.AddPolicy("authpolicy", app => app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            //});
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
