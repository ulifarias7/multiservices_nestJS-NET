using Auth.API.Middleware;
using Auth.API.Models.Dto;
using AutoMapper;
using Keycloak.Net;
using Keycloak.Net.Models.Groups;
using Keycloak.Net.Models.RealmsAdmin;
using Keycloak.Net.Models.Roles;
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

        public async Task<string> CreateUserKeycloack(CreateUserKeycloacDto dto, string realms)
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

                if (!string.IsNullOrEmpty(dto.GroupId))
                {
                    var group = await _keycloakClient.GetGroupAsync(realms, dto.GroupId);

                    await _keycloakClient.UpdateUserGroupAsync(
                        realms,
                        createdUser.Id,
                        dto.GroupId,
                        group
                    );
                }

                if (!string.IsNullOrEmpty(dto.RoleName))
                {
                    var role = await _keycloakClient.GetRoleByNameAsync(realms, dto.RoleName);

                    await _keycloakClient.AddRealmRoleMappingsToUserAsync(
                        realms,
                        createdUser.Id,
                        new[] { role }
                    );
                }

                return createdUser.Id;
            });
        }

        public async Task<bool> AssignRoleToGroupAsync(string realm, string groupId, string roleName)
        {
            return await _keycloakWrapper.Execute(async () =>
            {
                var role = await _keycloakClient.GetRoleByNameAsync(realm, roleName);

                await _keycloakClient.AddRealmRoleMappingsToGroupAsync(
                    realm,
                    groupId,
                    new[] { role }
                );

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
                if (!string.IsNullOrEmpty(dto.ParentGroupId))
                {
                    return await _keycloakClient.SetOrCreateGroupChildAsync(
                        realm: dto.Realms,
                        groupId: dto.ParentGroupId,
                        group: group
                    );
                }

                return await _keycloakClient.CreateGroupAsync(
                    realm: dto.Realms,
                    group: group
                );
            });
        }

        public async Task<CreateRealmDto> CreateRealm(CreateRealmDto dto)
        {
            return await _keycloakWrapper.Execute(async () =>
            {
                var realm = new Realm
                {
                    _Realm = dto.Realm,
                    Enabled = dto.Enabled
                };

                var success = await _keycloakClient.ImportRealmAsync(
                    realm: "master",
                    rep: realm
                );

                if (!success)
                    throw new Exception("Error creando el realm en Keycloak");

                return dto;
            });
        }

        public async Task<RealmsDto> GetRealm(RealmsDto dto)
        {
            var realm = new Realm()
            {
                Id = dto.Id,
                _Realm = dto.Realm,
                Enabled = dto.Enabled
            };

            return await _keycloakWrapper.Execute(async () =>
            {
                await _keycloakClient.GetRealmAsync(dto.Realm);
                return dto;
            });
        }

        public async Task<IEnumerable<RealmsDto>> GetRealms()
        {
            var realm = "master";
            return await _keycloakWrapper.Execute(async () =>
            {
                var realms = await _keycloakClient.GetRealmsAsync(realm);
                return _mapper.Map<IEnumerable<RealmsDto>>(realms);
            });
        }

        public async Task<IEnumerable<GroupsDto>> GroupsGet(string realm)
        {
            return await _keycloakWrapper.Execute(async () =>
            {
                var groups = await _keycloakClient.GetGroupHierarchyAsync(
                    realm,
                    first: null,
                    max: null,
                    search: null
                );

                var result = new List<GroupsDto>();
                foreach (var group in groups)
                {
                    var dto = _mapper.Map<GroupsDto>(group);
                    dto.SubGroups = await GetSubGroupsRecursive(realm, group.Id);
                    result.Add(dto);
                }

                return result;
            });
        }

        private async Task<List<GroupsDto>> GetSubGroupsRecursive(string realm, string groupId)
        {
            var children = await _keycloakClient.GetGroupChildrenAsync(realm, groupId);
            var result = new List<GroupsDto>();

            foreach (var child in children)
            {
                var dto = _mapper.Map<GroupsDto>(child);
                dto.SubGroups = await GetSubGroupsRecursive(realm, child.Id);
                result.Add(dto);
            }

            return result;
        }

        public async Task<GroupsDto> GroupById(string realm,string groupId)
        {
            return await _keycloakWrapper.Execute(async () =>
            {
                var group = await _keycloakClient.GetGroupAsync(
                    realm,
                    groupId
                );

                return _mapper.Map<GroupsDto>(group);
            });
        }

        public async Task<bool> CreateRealmRoleAsync(string realm, string roleName)
        {
            var role = new Role
            {
                Name = roleName
            };

            return await _keycloakWrapper.Execute(async () =>
            {
                return await _keycloakClient.CreateRoleAsync(realm, role);
            });
        }

        public async Task<bool> AddUserToGroupAsync(string realm, string userId, string groupId)
        {
            return await _keycloakWrapper.Execute(async () =>
            {
                var group = await _keycloakClient.GetGroupAsync(
                        realm,
                        groupId
                    );

                await _keycloakClient.UpdateUserGroupAsync(
                    realm,
                    userId,
                    groupId,
                    group
                );

                return true;
            });
        }
    }
}

//sidrel - one(Realm)
//│
//├── Roles(Realm Roles)
//│   ├── empresa_admin
//│   ├── empresa_usuario
//│   ├── empresa_visualizador
//│   ├── ministerio_admin
//│   ├── ministerio_inspector
//│   └── ministerio_usuario
//│
//├── Groups
//│   ├── empresas
//│   │   └── empresa-123
//│   │       ├── admin  ← Usuario "juan.perez" se asigna AQUÍ
//│   │       │   └── Roles asignados al grupo: [empresa_admin]
//│   │       ├── usuario
//│   │       │   └── Roles: [empresa_usuario]
//│   │       └── visualizador
//│   │           └── Roles: [empresa_visualizador]
//│   │
//│   └── ministerio
//│       ├── admin
//│       ├── inspector
//│       └── usuario
//│
//└── Users
//    └── juan.perez
//        ├── Email: juan @empresa123.com
//        ├── Groups: [/empresas/empresa-123/admin]  ← Pertenece a este grupo
//        └── Effective Roles: [empresa_admin]  ← Heredado del grupo

//¿Por qué así?
//Porque después podés preguntar cosas como:
//“¿Este usuario es admin de esta empresa?”
//“¿Pertenece al ministerio?”
//“¿Es inspector?”
//sin lógica rara.

//agregar validaciones
//agregaria una base de datos para poder filtar datos de id de manera mas facil (ya que keycloack no muestra los id de los grupos si no haces una pegada a la api)