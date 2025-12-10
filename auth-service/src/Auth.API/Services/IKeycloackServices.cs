using Auth.API.Models;

namespace Auth.API.Services
{
    public interface IKeycloackServices
    {
        Task<bool> CreateUserKeycloack(CreateUserKeycloacDto dto);
        Task<UserKeycloackDto> GetUserKeycloack(string id);
        Task<bool> DeleteUserKeycloak(string id);
        Task<bool> ResetPassword(ResetPasswordDto body);
    }
}
