using Microsoft.AspNetCore.Mvc;
using RemoteTest.Core;
using RemoteTest.Core.Dto;

namespace RemoteTest.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeterReadingController : ControllerBase
    {
        private readonly ILogger<MeterReadingController> _logger;
        private readonly IMeterReadingService meterReadingService;

        public MeterReadingController(ILogger<MeterReadingController> logger,
            IMeterReadingService meterReadingService)
        {
            _logger = logger;
            this.meterReadingService = meterReadingService;
        }

        [HttpGet("meter-reading")]
        public async Task<ActionResult<IEnumerable<MeterReadingDto>>> Get()
        {
            var result = await this.meterReadingService.GetMeterReadings();

            return Ok(result);
        }

        [HttpPost("meter-reading-uploads")]
        public async Task<ActionResult<MeterReadingsUploadResult>> Upload([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            var result = await this.meterReadingService.UploadMeterReadingsFile(file.OpenReadStream());

            return Ok(result);
        }
    }
}
