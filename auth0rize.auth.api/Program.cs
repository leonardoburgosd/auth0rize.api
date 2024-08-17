using auth0rize.auth.api.Extensions;
using auth0rize.auth.application;
using auth0rize.auth.infraestructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services
                .AddPresentation(configuration)
                .AddInjectionApplication()
                .AddInjectionInfraestructure(configuration)
                ;

builder.Services.AddControllers();

var app = builder.Build();
app.UseForwardedHeaders();
app.UseSwagger();
app.UseSwaggerUI();

app.useErrorHandlingMiddleware();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("auth0rizeapi");
app.UseAuthorization();

app.MapControllers();
app.Run();