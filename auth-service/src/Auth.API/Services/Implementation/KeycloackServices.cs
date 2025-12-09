using Auth.API.Models;
using Keycloak.Net;

namespace Auth.API.Services.Implementation
{
    public class KeycloackServices : IKeycloackServices
    {
        private readonly KeycloakClient _keycloakClient;
        private string _realm;

        public KeycloackServices(KeycloakClient keycloakClient)
        {
            _keycloakClient = keycloakClient;
        }

        public async Task<bool> CreateUserKeycloack(CreateUserKeycloacDto dto)
        {
            var result = await _keycloakClient.CreateUserAsync(_realm,dto);
        }
    }
}
