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
            var user = await _keycloakWrapper.Execute(async () =>
            {
                await _keycloakClient.GetUserAsync(realms, id);
            }); 

            return _mapper.Map<UserKeycloackDto>(user);
        }

        public async Task<bool> DeleteUserKeycloak(string id, string realms)
        {
            return await _keycloakWrapper.Execute(async () =>
            {
                return await _keycloakClient.DeleteUserAsync(realms, id);
            }); 
        }

        public async Task<bool> ResetPassword(ResetPasswordDto body, string realms)
        {
            return await _keycloakWrapper.Execute(async () =>
            {
                var credential = new Credentials
                {
                    Type = "password",
                    Value = body.NewPassword,
                    Temporary = body.Temporary
                };

                return await _keycloakClient.ResetUserPasswordAsync(
                    realms,
                    body.UserId,
                    credential
                );
            });        
        }

        public async Task<string> UpdateUserKeycloak(UpdateUserKeycloakDto body, string realms)
        {
            return await _keycloakWrapper.Execute(async () =>
            {
                var user = _mapper.Map<User>(body);
                var update = await _keycloakClient.UpdateUserAsync(realms, body.Id, user);
                
                return update
                ? "el usuario se actualizo correctamente" 
                : "no se pudo actualizar el usuario";
            });
        }

        public async Task<IEnumerable<UserKeycloackDto>> SearchUsers(
        string realm,
        string? search = null,
        string? email = null,
        string? username = null,
        string? firstName = null,
        string? lastName = null)
        {
            return await _keycloakWrapper.Execute(async () =>
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
            });
        }
    }
}
