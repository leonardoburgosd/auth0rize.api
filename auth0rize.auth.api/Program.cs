using auth0rize.auth.api.Extensions;
using auth0rize.auth.application;
using auth0rize.auth.infraestructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services
                .AddInjectionApplication()
                .AddInjectionInfraestructure(configuration)
                .AddPresentation(configuration)
                ;

builder.Services.AddControllers();
builder.Services.AddControllers();

var app = builder.Build();
app.UseForwardedHeaders();
app.UseSwagger();
app.UseSwaggerUI();

app.useErrorHandlingMiddleware();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthorization();

app.MapControllers();
app.Run();