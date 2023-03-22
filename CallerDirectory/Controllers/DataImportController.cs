using CallerDirectory.Services;
using Microsoft.AspNetCore.Mvc;

namespace CallerDirectory.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataImportController : Controller
    {
        private readonly IDataUploadService _dataUploadService;

        public DataImportController(IDataUploadService dataUploadService)
        {
            this._dataUploadService = dataUploadService;
        }

        [HttpPost]
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
