using RemoteTest.Core.Dto;

namespace RemoteTest.Core
{
    public interface IMeterReadingService
    {
        Task<IEnumerable<MeterReadingDto>> GetMeterReadings();
        Task<MeterReadingsUploadResult> UploadMeterReadingsFile(Stream stream);
    }
}
