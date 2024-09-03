﻿using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using System.Net;
using System.Text.Json;

namespace auth0rize.auth.api.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next) { _next = next; }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>() { Success = false, Message = error?.Message };
                switch (error)
                {
                    case ApiException e:
                        response.StatusCode = (int)HttpStatusCode.OK;
                        break;
                    case ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Errors = e.Errors;
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = new LowercaseNamingPolicy(),
                    WriteIndented = true
                });

                await response.WriteAsync(result);
            }
        }
    }
    public class LowercaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name.ToLower();
        }
    }
}
