using Document.API.Common.Exceptions;
using System;
using System.Net;
using System.Text.Json;

namespace Document.API.Common.Middleware
{
    public class ExcetionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExcetionMiddleware> _logger;

        public ExcetionMiddleware(RequestDelegate next, ILogger<ExcetionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext) 
        {
            try
            {
               await _next(httpContext);
            }
            catch(Exception ex)
            {
                await HandlerExceptionAsync(httpContext,ex);
            }
        }

        private async Task HandlerExceptionAsync(HttpContext httpContext, Exception ex)
        {
            _logger.LogError(ex, $"Error capturado.");

            var response = httpContext.Response;
            response.ContentType = "application/json";

            HttpStatusCode status;
            string message = ex.Message;

            switch (ex)
            {
                case BadHttpRequestException:
                    status = HttpStatusCode.BadRequest;
                    break;

                case NotFoundException:
                    status = HttpStatusCode.NotFound;
                    break;

                case ForbiddenException:
                    status = HttpStatusCode.Forbidden;
                    break;

                case MinioException:
                    status = HttpStatusCode.InternalServerError;
                    break;

                default:
                    status = HttpStatusCode.InternalServerError;
                    message = "Internal Server Error.";
                    break;
            }

            response.StatusCode = (int)status;

            var json = JsonSerializer.Serialize(new
            {
                Code = response.StatusCode,
                Message = message,
                Error = ex.GetType().Name
            });

            await response.WriteAsync(json);
        }
    }
}
