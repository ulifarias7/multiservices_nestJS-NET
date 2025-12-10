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
        public async Task<ActionResult> CreateUserKeycloack([FromForm] CreateUserKeycloacDto dto)
        {
            var result = await _keyclaockService.CreateUserKeycloack(dto);
            return Ok(result);
        }

        [HttpPost("get-user-keycloak")]
        public async Task<ActionResult> GetUserKeycloack([FromQuery] string id)
        {
            var result = await _keyclaockService.GetUserKeycloack(id);
            return Ok(result);
        }

        [HttpPost("delete-user-keycloak")]
        public async Task<ActionResult> DeleteUserKeycloak([FromForm] string id)
        {
            var result = await _keyclaockService.DeleteUserKeycloak(id);
            return Ok(result);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto body) 
        {
            var result = await _keyclaockService.ResetPassword(body);
            return Ok(result);
        }
    }
}
