using Auth.API.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;

namespace Auth.API.Middleware
{
    public class KeycloakExceptionMiddleware
    {
        private readonly RequestDelegate _next; // referrencia al siguiente elemento del pipeline 
       //Cuando este middleware termine su trabajo debe llamar a _next(context) para que la request continúe.
        private readonly ILogger<KeycloakExceptionMiddleware> _logger; // es para que el logging sea contextualizado 

        public KeycloakExceptionMiddleware(RequestDelegate next, ILogger<KeycloakExceptionMiddleware> logger)
        {
            _next = next;// el sigueinte middleware en la cadena 
            _logger = logger; // logger proporcionado
        }

        //Este es el método que ASP.NET Core invoca por cada request. HttpContext trae TODO sobre la petición y la respuesta (headers, body, user, path, etc.).
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // eso significa "dejo que la request continue"
            }
            catch (KeycloakException ex) // si tira alguna exception la captura y devuelve el modelo 
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
            ctx.Response.StatusCode = (int)code; // setea el codigo de respuesta 
            ctx.Response.ContentType = "application/json"; // setea el tipo de contenido de respuesta 
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(body));  // serealiza el objeto a json 
        }
    }
}
