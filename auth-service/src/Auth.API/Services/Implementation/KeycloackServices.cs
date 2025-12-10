using Auth.API.Models;
using Keycloak.Net;
using Keycloak.Net.Models.Users;

namespace Auth.API.Services.Implementation
{
    public class KeycloackServices : IKeycloackServices
    {
        private readonly KeycloakClient _keycloakClient;
        private readonly IConfiguration _configuration;
        private readonly string _realm;

        public KeycloackServices(KeycloakClient keycloakClient,
            IConfiguration configuration)
        {
            _keycloakClient = keycloakClient;
            _configuration = configuration;
            _realm = _configuration["Keycloak:Realm"]!;
        }

        public async Task<bool> CreateUserKeycloack(CreateUserKeycloacDto dto)
        {
            var user = new Keycloak.Net.Models.Users.User
            {
                UserName = dto.UserName ?? dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                EmailVerified = dto.EmailVerified ?? false,
                Enabled = dto.Enabled ?? true,
                Credentials = new List<Credentials>
                {
                    new Credentials
                    {
                        Type = "password",
                        Value = dto.CredentialDto.Value,
                        Temporary = dto.CredentialDto.Temporary ?? false
                    }
                }
            };

            var result = await _keycloakClient.CreateUserAsync(_realm, user);
            return result;
        }

        public async Task<UserKeycloackDto> GetUserKeycloack(string id)
        {
            var user = await _keycloakClient.GetUserAsync(_realm, id);

            var userDto = new UserKeycloackDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return userDto;
        }
        public async Task<bool> DeleteUserKeycloak(string id)
        {
            var userDrop = await _keycloakClient.DeleteUserAsync(_realm, id);
            return userDrop;
        }

        public async Task<bool> ResetPassword(ResetPasswordDto body)
        {
            var credential = new Credentials
            {
                Type = "password",
                Value = body.NewPassword,
                Temporary = body.Temporary
            };

            var result = await _keycloakClient.ResetUserPasswordAsync(
                _realm,
                body.UserId,
                credential
            );

            return result;
        }
    }
}
