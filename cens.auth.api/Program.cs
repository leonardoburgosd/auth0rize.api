using cens.auth.api.Extensions;
using cens.auth.application;
using cens.auth.infraestructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services
                .AddInjectionApplication()
                .AddInjectionInfraestructure(configuration)
                .AddPresentation(configuration)
                ;
builder.Services.AddCors(p => p.AddPolicy("authpolicy",
        //app => app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
        app =>
        {
            app.WithOrigins(
                "https://workforce.censperu.com",
                "https://auth.censperu.com"
            ).AllowAnyOrigin().AllowAnyMethod().AllowCredentials();
        }
    ));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseForwardedHeaders();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("authpolicy");
app.UseRouting();
app.useErrorHandlingMiddleware();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();

