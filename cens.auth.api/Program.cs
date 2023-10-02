using cens.auth.api.Extensions;
using cens.auth.application.Extensions;
using cens.auth.infraestructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



var configuration = builder.Configuration;
builder.Services.AddControllers();
builder.Services.AddInjectionApplication(configuration)
                .AddInjectionInfraestructure(configuration);

builder.Services.AddApiVersioningExtension();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(p => p.AddPolicy("corspolicy",
        app => app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
        //build =>
        //{
        //    List<string> corsList = configuration.GetSection("cors")!.Get<string[]>().ToList();
        //    build.WithOrigins(string.Join(",", corsList)).AllowAnyMethod().AllowAnyHeader();
        //}
    ));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(4);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
    AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["security:issuer"]!.ToString(),
        ValidAudience = configuration["security:audience"]!.ToString(),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["security:symmetricKey"]!.ToString())),
        ClockSkew = TimeSpan.Zero
    });

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSession();

app.UseCors("corspolicy");
app.useErrorHandlingMiddleware();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
