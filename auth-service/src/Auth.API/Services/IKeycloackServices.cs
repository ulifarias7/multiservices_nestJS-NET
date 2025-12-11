using Auth.API.Models;

namespace Auth.API.Services
{
    public interface IKeycloackServices
    {
        Task<bool> CreateUserKeycloack(CreateUserKeycloacDto dto, string realms);
        Task<UserKeycloackDto> GetUserKeycloack(string id, string realms);
        Task<bool> DeleteUserKeycloak(string id, string realms);
        Task<bool> ResetPassword(ResetPasswordDto body, string realms);
        Task<string> UpdateUserKeycloak(UpdateUserKeycloakDto body, string realms);
        Task<IEnumerable<UserKeycloackDto>> SearchUsers(string realm,
        string? search = null,
        string? email = null,
        string? username = null,
        string? firstName = null,
        string? lastName = null);
    }
}
