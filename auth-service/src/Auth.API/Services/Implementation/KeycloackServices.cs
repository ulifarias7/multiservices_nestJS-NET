using Auth.API.Middleware;
using Auth.API.Models.Dto;
using AutoMapper;
using Keycloak.Net;
using Keycloak.Net.Models.Users;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Auth.API.Services.Implementation
{
    public class KeycloackServices : IKeycloackServices
    {
        private readonly KeycloakClient _keycloakClient;
        private readonly IMapper _mapper;
        private readonly KeycloakWrapper _keycloakWrapper;
        private HttpClient _HttpClient;

        public KeycloackServices(
            KeycloakClient keycloakClient,
            IMapper mapper,
            KeycloakWrapper keycloakWrapper,
            HttpClient httpClient)
        {
            _keycloakClient = keycloakClient;
            _mapper = mapper;
            _keycloakWrapper = keycloakWrapper;
            _HttpClient = httpClient;
        }

        public async Task<bool> CreateUserKeycloack(CreateUserKeycloacDto dto, string realms)
        {
            var user = _mapper.Map<User>(dto);

            return await _keycloakWrapper.Execute(async () =>
            {
                await _keycloakClient.CreateUserAsync(realms, user);

                var users = await _keycloakClient.GetUsersAsync(
                    realms,
                    username: dto.UserName
                );

                var createdUser = users.FirstOrDefault()
                    ?? throw new Exception("No se pudo obtener el usuario creado");

                var token = await GetAdminTokenAsync();

                _HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                foreach (var groupId in dto.GroupIds)
                {
                    var response = await _HttpClient.PutAsync(
                        $"/admin/realms/{realms}/users/{createdUser.Id}/groups/{groupId}",
                        null
                    );

                    if (!response.IsSuccessStatusCode)
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        throw new Exception(
                            $"Error al asignar el usuario al grupo {groupId}: {error}"
                        );
                    }
                }

                return true;
            });
        }

        public async Task<UserKeycloackDto> GetUserKeycloack(string id, string realms)
        {
            return await _keycloakWrapper.Execute(async () =>
            {
                var user = await _keycloakClient.GetUserAsync(realms, id);
                return _mapper.Map<UserKeycloackDto>(user);
            });
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

                return await _keycloakWrapper.Execute(async () =>
                {
                    return await _keycloakClient.ResetUserPasswordAsync(
                        realms,
                        body.UserId,
                        credential
                    );
                });
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

        public async Task<bool> CreateGroupAsync(CreateGroupDto dto)
        {
            var group = new Keycloak.Net.Models.Groups.Group
            {
                Name = dto.Name
            };

            return await _keycloakWrapper.Execute(async () =>
            {
                var result = await _keycloakClient.CreateGroupAsync(
                    realm: dto.Realms,
                    group
                );

                return result;
            });
        }

        public async Task<CreateRealmDto> CreateRealm(CreateRealmDto dto)
        {
            return await _keycloakWrapper.Execute(async () =>
            {
                var token = await GetAdminTokenAsync();

                _HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _HttpClient.PostAsJsonAsync(
                    "/admin/realms",
                    new
                    {
                        realm = dto.Realm,
                        enabled = dto.Enabled
                    }
                );

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error creando realm: {error}");
                }

                return dto;
            });
        }

        private async Task<string> GetAdminTokenAsync()
        {
            var form = new Dictionary<string, string>
            {
                ["client_id"] = "admin-cli",
                ["grant_type"] = "password",
                ["username"] = "admin",
                ["password"] = "admin"
            };

            var response = await _HttpClient.PostAsync(
                "/realms/master/protocol/openid-connect/token",
                new FormUrlEncodedContent(form)
            );

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();

            return json.GetProperty("access_token").GetString()!;
        }

        public async Task<IEnumerable<RealmsDto>> GetRealms()
        {
            return await _keycloakWrapper.Execute(async () =>
            {
                var token = await GetAdminTokenAsync();

                _HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _HttpClient.GetAsync("/admin/realms");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error obteniendo realms: {error}");
                }

                var realms = await response.Content
                    .ReadFromJsonAsync<List<RealmsDto>>();

                return realms!;
            });
        }

        public async Task<IEnumerable<GroupsDto>> GroupsGet(string realms)
        {
            return await _keycloakWrapper.Execute(async () =>
            {
                var token = await GetAdminTokenAsync();

                _HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _HttpClient.GetAsync($"/admin/realms/{realms}/groups"); 

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error obteniendo grupos: {error}");
                }

                var groups = await response.Content
                    .ReadFromJsonAsync<List<GroupsDto>>();

                return groups!;
            });
        }
    }
}
