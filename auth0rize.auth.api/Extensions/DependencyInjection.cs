using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace auth0rize.auth.api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSwaggerGen();

            services.AddCors(p =>
            {
                p.AddPolicy("AllowSpecificOrigin",
                    builder => builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
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
