using RemoteTest.Core.Dto;

namespace RemoteTest.Core
{
    public interface IMeterReadingService
    {
        Task<IEnumerable<MeterReadingViewDto>> GetMeterReadings();
        Task<MeterReadingsUploadResult> UploadMeterReadingsFile(Stream stream);
    }
}
