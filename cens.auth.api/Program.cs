using cens.auth.api.Extensions;
using cens.auth.application;
using cens.auth.drive;
using cens.auth.infraestructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services
                .AddInjectionApplication()
                .AddInjectionDrive(configuration)
                .AddInjectionInfraestructure(configuration)
                .AddPresentation(configuration)
                ;

builder.Services.AddControllers();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseForwardedHeaders();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.useErrorHandlingMiddleware();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthorization();

app.MapControllers();
app.Run();

