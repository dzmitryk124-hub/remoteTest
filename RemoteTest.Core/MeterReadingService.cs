using RemoteTest.Core.Dto;

namespace RemoteTest.Core
{
    public sealed class MeterReadingService : IMeterReadingService
    {
        public async Task<IEnumerable<MeterReadingDto>> GetMeterReadings()
        {
            throw new NotImplementedException();
        }

        public async  Task<IEnumerable<MeterReadingsUploadResult>> UploadMeterReadingsFile()
        {
            throw new NotImplementedException();
        }
    }
}
