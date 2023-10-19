using cens.auth.api.Middlewares;

namespace cens.auth.api.Extensions
{
    public static class AppExtensions
    {
        public static void useErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
