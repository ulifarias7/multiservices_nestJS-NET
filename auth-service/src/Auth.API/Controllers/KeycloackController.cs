using Auth.API.Models.Dto;
using Auth.API.Services;
using Keycloak.Net.Core.Models.Root;
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

        [HttpPost("reset-password-user-keycloak")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto body, [FromQuery] string realms)
        {
            var result = await _keyclaockService.ResetPassword(body, realms);
            return Ok(result);
        }

        [HttpPost("update-user-keycloak")]
        public async Task<ActionResult> UpdateUserKeyclaock([FromBody] UpdateUserKeycloakDto body, [FromQuery] string realms)
        {
            var result = await _keyclaockService.UpdateUserKeycloak(body, realms);
            return Ok(result);
        }

        [HttpPost("serch-user-keycloak")]
        public async Task<ActionResult> SerchUserKeyclok(
        [FromQuery] string realm,
        [FromQuery] string? search,
        [FromQuery] string? email,
        [FromQuery] string? username,
        [FromQuery] string? firstName,
        [FromQuery] string? lastName)
        {
            var result = await _keyclaockService.SearchUsers(realm, search, email, username, firstName, lastName);
            return Ok(result);
        }

        [HttpPost("create-group-keycloak")]
        public async Task<ActionResult> CreateGroup([FromBody] CreateGroupDto dto)
        {
            var result = await _keyclaockService.CreateGroupAsync(dto);
            return Ok(result);
        }

        [HttpPost("create-realms-keycloak")]
        public async Task<ActionResult> CreateRealm([FromBody] CreateRealmDto dto)
        {
            var result = await _keyclaockService.CreateRealm(dto);
            return Ok(result);
        }

        [HttpPost("create-role-keycloak")]
        public async Task<ActionResult> CreateRole([FromForm] string realm, [FromForm] string role)
        {
            var result = await _keyclaockService.CreateRealmRoleAsync(realm, role);
            return Ok(result);
        }

        [HttpPost("add-user-to-group-keycloak")]
        public async Task<ActionResult> AddUserToGroupAsync([FromQuery] string realm, [FromQuery] string userId, [FromQuery] string groupId)
        {
            var result = await _keyclaockService.AddUserToGroupAsync(realm, userId, groupId);
            return Ok(result);
        }

        [HttpPost("assign-role-to-group-keycloak")]
        public async Task<ActionResult> AssignRoleToGroup(
        [FromQuery] string realm,
        [FromQuery] string groupId,
        [FromQuery] string roleName)
        {
            var result = await _keyclaockService.AssignRoleToGroupAsync(realm, groupId, roleName);
            return Ok(result);
        }

        [HttpGet("get-realms-keycloak")]
        public async Task<ActionResult> GetRealms()
        {
            var result = await _keyclaockService.GetRealms();
            return Ok(result);
        }

        [HttpGet("get-all-groups-by-realm-keycloak")]
        public async Task<ActionResult> GroupsGet([FromQuery] string realms)
        {
            var result = await _keyclaockService.GroupsGet(realms);
            return Ok(result);
        }

        [HttpGet("get-group-by-id-keycloak")]
        public async Task<ActionResult> GroupGet([FromQuery] string realms, [FromQuery] string groupId)
        {
            var result = await _keyclaockService.GroupById(realms,groupId);
            return Ok(result);
        }
    }
}
