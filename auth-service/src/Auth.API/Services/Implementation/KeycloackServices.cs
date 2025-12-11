using Auth.API.Middleware;
using Auth.API.Models;
using AutoMapper;
using Keycloak.Net;
using Keycloak.Net.Models.Users;

namespace Auth.API.Services.Implementation
{
    public class KeycloackServices : IKeycloackServices
    {
        private readonly KeycloakClient _keycloakClient;
        private readonly IMapper _mapper;
        private readonly KeycloakWrapper _keycloakWrapper;

        public KeycloackServices(
            KeycloakClient keycloakClient,
            IMapper mapper,
            KeycloakWrapper keycloakWrapper)
        {
            _keycloakClient = keycloakClient;
            _mapper = mapper;
            _keycloakWrapper = keycloakWrapper;
        }

        public async Task<bool> CreateUserKeycloack(CreateUserKeycloacDto dto, string realms)
        {
            var user = _mapper.Map<User>(dto);

            return await _keycloakWrapper.Execute(async () =>
            {
                return await _keycloakClient.CreateUserAsync(realms, user);

                //  emitir evento , hacer pegada a user-service ?
            });
        }

        public async Task<UserKeycloackDto> GetUserKeycloack(string id, string realms)
        {
            var user = await _keycloakClient.GetUserAsync(realms, id);
            var userDto = _mapper.Map<UserKeycloackDto>(user);
            return userDto;
        }
        public async Task<bool> DeleteUserKeycloak(string id, string realms)
        {
            var userDrop = await _keycloakClient.DeleteUserAsync(realms, id);
            return userDrop;
        }

        public async Task<bool> ResetPassword(ResetPasswordDto body, string realms)
        {
            var credential = new Credentials
            {
                Type = "password",
                Value = body.NewPassword,
                Temporary = body.Temporary
            };

            var result = await _keycloakClient.ResetUserPasswordAsync(
                realms,
                body.UserId,
                credential
            );

            return result;
        }

        public async Task<string> UpdateUserKeycloak(UpdateUserKeycloakDto body, string realms)
        {
            var user = _mapper.Map<User>(body);
            var update = await _keycloakClient.UpdateUserAsync(realms, body.Id, user);

            if (update)
            {
                return "Se edito el usuario con exitosamente.";
            }

            return "No se pudo editar el usuario.";
        }

        public async Task<IEnumerable<UserKeycloackDto>> SearchUsers(
        string realm, string? search = null, string? email = null, string? username = null,
        string? firstName = null, string? lastName = null)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                username = search;
            }

            var users = await _keycloakClient.GetUsersAsync(
                realm,
                email: email,
                firstName: firstName,
                lastName: lastName,
                username: username
            );

            return _mapper.Map<IEnumerable<UserKeycloackDto>>(users);
        }
    }
}
