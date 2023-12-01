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
builder.Services.AddCors(p => p.AddPolicy("corspolicy",
        app => app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
    ));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseForwardedHeaders();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("authpolicy");
app.useErrorHandlingMiddleware();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();

