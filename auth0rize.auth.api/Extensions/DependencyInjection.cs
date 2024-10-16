﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace auth0rize.auth.api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSwaggerGen(s =>
            {
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    Name = "Authorization"
                });
                s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                     }
                });
            });
            string? origins = Environment.GetEnvironmentVariable(configuration["security:origins"]!.ToString());
            string? symmetricKey = Environment.GetEnvironmentVariable(configuration["security:symmetricKey"]!.ToString());
            string? issuer = Environment.GetEnvironmentVariable(configuration["security:issuer"]!.ToString());
            string? audience = Environment.GetEnvironmentVariable(configuration["security:audience"]!.ToString());

            if (origins is null) throw new KeyNotFoundException("Origins no establecidos.");
            if (symmetricKey is null) throw new KeyNotFoundException("SymmetricKey no establecido.");
            if (issuer is null) throw new KeyNotFoundException("Issuer no establecido.");
            if (audience is null) throw new KeyNotFoundException("Audience no establecido.");

            services.AddCors(p =>
            {
                p.AddPolicy("auth0rizeapi",
                    builder => builder.WithOrigins(origins)
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowCredentials()
                );
            });

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