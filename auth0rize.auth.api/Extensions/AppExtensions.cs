using auth0rize.auth.api.Middlewares;

namespace auth0rize.auth.api.Extensions
{
    public static class AppExtensions
    {
        public static void useErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
