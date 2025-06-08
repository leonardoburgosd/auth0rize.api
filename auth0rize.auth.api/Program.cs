using auth0rize.auth.api.Extensions;
using auth0rize.auth.application;
using auth0rize.auth.infraestructure.Extensions;
using auth0rize.auth.notification;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services
                .AddInjectionApplication()
                .AddInjectionInfraestructure(configuration)
<<<<<<< Updated upstream
<<<<<<< Updated upstream
                .AddPresentation(configuration)
=======
                .AddInjectionNotification(configuration)
>>>>>>> Stashed changes
=======
                .AddInjectionNotification(configuration)
>>>>>>> Stashed changes
                ;

builder.Services.AddControllers();

var app = builder.Build();
app.UseForwardedHeaders();
app.UseSwagger();
app.UseSwaggerUI();

app.useErrorHandlingMiddleware();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();

app.MapControllers();
app.Run();