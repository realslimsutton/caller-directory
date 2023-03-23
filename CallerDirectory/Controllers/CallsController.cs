using CallerDirectory.DataAccess;
using CallerDirectory.Models;
using CallerDirectory.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

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
        public async Task<IActionResult> GetRecordAsync(string reference)
        {
            try
            {
                CallRecord? record = await this._callRecordsService.GetRecordAsync(reference);

                if (record != null)
                {
                    return Json(new Response<CallRecord>(record));
                }

                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("/HourlyCosts")]
        public async Task<IActionResult> GetHourlyCosts()
        {
            try
            {
                IEnumerable<object> records = await this._callRecordsService.GetHourlyCostsAsync();

                return Json(new Response<IEnumerable<object>>(records));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("/")]
        public async Task<IActionResult> GetRecordsAsync([FromQuery]PaginatedRequest pagination)
        {
            try
            {
                IEnumerable<CallRecord> records = await this._callRecordsService.GetRecordsAsync(pagination);

                return Json(new PaginatedResponse<CallRecord>(records, pagination));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("/caller/unknown")]
        public async Task<IActionResult> GetUnknownCallerRecords([FromQuery] PaginatedRequest pagination)
        {
            try
            {
                IEnumerable<CallRecord> records = await this._callRecordsService.GetCallerRecordsAsync(pagination);

                return Json(new PaginatedResponse<CallRecord>(records, pagination));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("/caller/{callerId?}")]
        public async Task<IActionResult> GetCallerRecords([FromQuery] PaginatedRequest pagination, [FromRoute]long? callerId = null)
        {
            try
            {
                IEnumerable<CallRecord> records = await this._callRecordsService.GetCallerRecordsAsync(pagination, callerId);

                return Json(new PaginatedResponse<CallRecord>(records, pagination));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("/import")]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
        public async Task<IActionResult> ImportAsync(IFormFile file)
        {
            Stream stream = file.OpenReadStream();

            try
            {
                await this._dataUploadService.ImportAsync(stream);
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
