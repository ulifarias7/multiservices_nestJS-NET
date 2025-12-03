using Document.API.Models.Request;
using Document.API.Models.Responses;
using Document.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Document.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileServices _fileServices;

        public FileController(IFileServices fileServices)
        {
            _fileServices = fileServices;
        }

        [HttpPost("create-bucket")]
        public async Task<ActionResult<string>> CreateBucket([FromQuery] string bucket)
        {
            var result = await _fileServices.CreateBucketAsync(bucket);
            return Ok(result);
        }

        [HttpPost("create-object")]
        public async Task<ActionResult<ResponsesObjectJson>> CreateObject(CreateObjectDto model)
        {
            var result = await _fileServices.CreateObjectAsync(model);
            return Ok(result);
        }

        [HttpGet("get-bucket-info")]
        public async Task<ActionResult<ResponsesObjectJson>> GetBucket([FromQuery] string bucket)
        {
            var result = await _fileServices.GeBucketAsync(bucket);
            return Ok(result);
        }

        [HttpGet("get-object-info")]
        public async Task<ActionResult<ResponsesObjectJson>> GetObject([FromQuery] string bucket, [FromQuery] string objectName)
        {
            var result = await _fileServices.GetObjectAsync(bucket, objectName);
            return Ok(result);
        }

        [HttpGet("download-object")]
        public async Task<IActionResult> DownloadObject([FromQuery] string bucket, [FromQuery] string objectName)
        {
            var (fileBytes, contentType, fileName) = await _fileServices.DownloadObject(bucket, objectName);
            return File(fileBytes, contentType, fileName);
        }
    }
}
