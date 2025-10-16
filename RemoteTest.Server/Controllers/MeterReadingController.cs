using Microsoft.AspNetCore.Mvc;
using RemoteTest.Core.Dto;
using RemoteTest.Core.Interfaces;

namespace RemoteTest.Server.Controllers
{
    [ApiController]
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
        public async Task<ActionResult<IEnumerable<MeterReadingViewDto>>> Get()
        {
            try
            {

                var result = await this.meterReadingService.GetMeterReadings();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed ${nameof(Get)}", ex);
                throw;
            }
        }

        [HttpPost("meter-reading-uploads")]
        public async Task<ActionResult<MeterReadingsUploadResult>> Upload([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                var result = await this.meterReadingService.UploadMeterReadingsFile(file.OpenReadStream());

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed ${nameof(Upload)}", ex);
                throw;
            }
        }
    }
}
