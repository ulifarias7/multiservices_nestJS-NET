using Auth.API.Models;
using Auth.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeycloackController : ControllerBase
    {
        private readonly IKeycloackServices _keyclaockService;

        public KeycloackController(IKeycloackServices keyclaockService)
        {
            _keyclaockService = keyclaockService;
        }

        [HttpPost("create-user-keycloak")]
        public async Task<ActionResult> CreateUserKeycloack([FromForm] CreateUserKeycloacDto dto, [FromForm] string realms)
        {
            var result = await _keyclaockService.CreateUserKeycloack(dto, realms);
            return Ok(result);
        }

        [HttpPost("get-user-keycloak")]
        public async Task<ActionResult> GetUserKeycloack([FromQuery] string id, [FromQuery] string realms)
        {
            var result = await _keyclaockService.GetUserKeycloack(id, realms);
            return Ok(result);
        }

        [HttpPost("delete-user-keycloak")]
        public async Task<ActionResult> DeleteUserKeycloak([FromForm] string id, [FromQuery] string realms)
        {
            var result = await _keyclaockService.DeleteUserKeycloak(id, realms);
            return Ok(result);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto body, [FromQuery] string realms)
        {
            var result = await _keyclaockService.ResetPassword(body, realms);
            return Ok(result);
        }

        [HttpPost("update-user-keyclok")]
        public async Task<ActionResult> UpdateUserKeyclaock([FromBody] UpdateUserKeycloakDto body, [FromQuery] string realms)
        {
            var result = await _keyclaockService.UpdateUserKeycloak(body, realms);
            return Ok(result);
        }

        [HttpPost("serch-user-keyclok")]
        public async Task<ActionResult> SerchUserKeyclok(
        [FromQuery] string realm,
        [FromQuery] string? search,
        [FromQuery] string? email,
        [FromQuery] string? username,
        [FromQuery] string? firstName,
        [FromQuery] string? lastName)
        {
            var result = await _keyclaockService.SearchUsers(realm,search,email,username,firstName,lastName);
            return Ok(result);
        }
    }
}
