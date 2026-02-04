using Auth.API.Models.Dto;

namespace Auth.API.Services
{
    public interface IKeycloackServices
    {
        Task<string> CreateUserKeycloack(CreateUserKeycloacDto dto, string realms);
        Task<UserKeycloackDto> GetUserKeycloack(string id, string realms);
        Task<bool> DeleteUserKeycloak(string id, string realms);
        Task<bool> ResetPassword(ResetPasswordDto body, string realms);
        Task<string> UpdateUserKeycloak(UpdateUserKeycloakDto body, string realms);
        Task<bool> CreateGroupAsync(CreateGroupDto dto);
        Task<CreateRealmDto> CreateRealm(CreateRealmDto dto);
        Task<IEnumerable<RealmsDto>> GetRealms();
        Task<IEnumerable<GroupsDto>> GroupsGet(string realms);
        Task<bool> CreateSubGroupAsync(CreateSubGroupDto dto);
        Task<bool> CreateRealmRoleAsync(string realm, string roleName);
        Task<bool> AddUserToGroupAsync(string realm, string userId, string groupId);
        Task<GroupsDto> GroupById(string realm, string groupId);
        Task<IEnumerable<UserKeycloackDto>> SearchUsers(string realm,
        string? search = null,
        string? email = null,
        string? username = null,
        string? firstName = null,
        string? lastName = null);
    }
}
