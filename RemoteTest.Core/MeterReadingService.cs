using RemoteTest.Core.Dto;

namespace RemoteTest.Core
{
    public sealed class MeterReadingService : IMeterReadingService
    {
        public async Task<IEnumerable<MeterReadingDto>> GetMeterReadings()
        {
            throw new NotImplementedException();
        }

        public async  Task<MeterReadingsUploadResult> UploadMeterReadingsFile(Stream stream)
        {
            var result = new MeterReadingsUploadResult 
            { 
                NumberOfFailedReadings = 0,
                NumberOfSuccessfulReadings = 0,
            };
            return await Task.FromResult(result); 
        }
    }
}
