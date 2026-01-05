using Auth.API.Models.Exceptions;
using System.Net;
using System.Text.Json;

namespace Auth.API.Middleware
{
    public class KeycloakExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<KeycloakExceptionMiddleware> _logger;

        public KeycloakExceptionMiddleware(RequestDelegate next, ILogger<KeycloakExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (KeycloakException ex)
            {
                _logger.LogError(ex, "KeycloakException handled");

                await WriteError(context, ex.StatusCode, new
                {
                    status = (int)ex.StatusCode,
                    message = ex.Message,
                    keycloakMessage = ex.KeycloakMessage,
                    raw = ex.RawError,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        private async Task WriteError(HttpContext ctx, HttpStatusCode code, object body)
        {
            ctx.Response.StatusCode = (int)code;
            ctx.Response.ContentType = "application/json";
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(body));
        }
    }
}
