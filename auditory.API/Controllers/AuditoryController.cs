using Auditory.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace auditory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditoryController : ControllerBase
    {
        private readonly IAuditServices _auditService;

        public AuditoryController(IAuditServices auditService)
        {
            _auditService = auditService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRegister([FromQuery] Guid id)
        {
            var result = await _auditService.GetRegister(id);
            return Ok(result);
        }
    }
}
