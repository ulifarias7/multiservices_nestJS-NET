using Auth.API.Models;

namespace Auth.API.Services
{
    public interface IKeycloackServices
    {
        Task<bool> CreateUserKeycloack(CreateUserKeycloacDto dto);
    }
}
