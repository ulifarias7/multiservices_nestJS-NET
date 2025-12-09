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

        [HttpPost("create-user-keycloac")]
        public async Task<ActionResult> CreateUserKeycloack([FromForm] CreateUserKeycloacDto dto) 
        { 
            var result = await _keyclaockService.CreateUserKeycloack(dto);
            return Ok(result);
        }
    }
}
