using System.Net;

namespace Auth.API.Models.Exceptions
{
    public class KeycloakException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string? KeycloakMessage { get; }
        public string? RawError { get; }

        public KeycloakException(
            HttpStatusCode statusCode,
            string message,
            string? keycloakMessage = null,
            string? rawError = null
        ) : base(message)
        {
            StatusCode = statusCode;
            KeycloakMessage = keycloakMessage;
            RawError = rawError;
        }
    }
}
