using CallerDirectory.DataAccess;
using CallerDirectory.Models;
using CallerDirectory.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CallerDirectory.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CallsController : Controller
    {
        private readonly ICallRecordsService _callRecordsService;

        private readonly IDataUploadService _dataUploadService;

        public CallsController(ICallRecordsService callRecordsService, IDataUploadService dataUploadService)
        {
            this._callRecordsService = callRecordsService;
            this._dataUploadService = dataUploadService;
        }

        [HttpGet("/{reference}")]
        public async Task<IActionResult> Get(string reference)
        {
            CallRecord? record = await this._callRecordsService.Get(reference);

            if(record == null)
            {
                return Json(record);
            }

            return NotFound();
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
