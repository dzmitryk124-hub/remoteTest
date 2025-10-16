using RemoteTest.Core.Dto;

namespace RemoteTest.Core
{
    public interface IMeterReadingService
    {
        Task<IEnumerable<MeterReadingDto>> GetMeterReadings();
        Task<IEnumerable<MeterReadingsUploadResult>> UploadMeterReadingsFile();
    }
}
