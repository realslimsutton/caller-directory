using CallerDirectory.Services;
using Microsoft.AspNetCore.Mvc;

namespace CallerDirectory.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CallsController : Controller
    {
        private readonly IConfiguration configuration;

        private readonly IDataUploadService _dataUploadService;

        public CallsController(IConfiguration configuration, IDataUploadService dataUploadService)
        {
            this.configuration = configuration;
            this._dataUploadService = dataUploadService;
        }

        [HttpPost("/import")]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
        public async Task<IActionResult> Import(IFormFile file)
        {
            Stream stream = file.OpenReadStream();

            try
            {
                await this._dataUploadService.Import(stream);
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
