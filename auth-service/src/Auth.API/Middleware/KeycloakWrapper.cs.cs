using Auth.API.Models.Exceptions;
using Keycloak.Net;
using System.Net;
using System.Text.Json;

namespace Auth.API.Middleware
{
    public class KeycloakWrapper
    {
        private readonly KeycloakClient _client;
        private readonly ILogger<KeycloakWrapper> _logger;

        public KeycloakWrapper(
            KeycloakClient client,
            ILogger<KeycloakWrapper> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<T> Execute<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Keycloak returned an HTTP error");

                var statusCode = ex.StatusCode ?? HttpStatusCode.InternalServerError;

                throw new KeycloakException(
                    statusCode,
                    "Keycloak devolvió un error HTTP.",
                    keycloakMessage: ex.Message
                );
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Invalid JSON from Keycloak");

                throw new KeycloakException(
                    HttpStatusCode.InternalServerError,
                    "Keycloak devolvió un JSON inválido.",
                    rawError: ex.Message
                );
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Timeout calling Keycloak");

                throw new KeycloakException(
                    HttpStatusCode.RequestTimeout,
                    "Keycloak no respondió a tiempo.",
                    rawError: ex.Message
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error calling Keycloak");

                throw new KeycloakException(
                    HttpStatusCode.InternalServerError,
                    "Error inesperado consultando Keycloak.",
                    rawError: ex.Message
                );
            }
        }
    }
}
