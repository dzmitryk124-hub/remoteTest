using Microsoft.AspNetCore.Mvc;
using RemoteTest.Core;
using RemoteTest.Core.Dto;

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
        public async Task<IEnumerable<MeterReadingDto>> Get()
        {
            return await this.meterReadingService.GetMeterReadings();
        }

        [HttpPost("meter-reading-uploads")]
        public async Task<IEnumerable<MeterReadingsUploadResult>> Upload()
        {
            return await this.meterReadingService.UploadMeterReadingsFile();
        }
    }
}
