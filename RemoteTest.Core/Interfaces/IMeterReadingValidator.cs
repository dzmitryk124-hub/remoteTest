using RemoteTest.Core.Dto;
using RemoteTest.Server.Entities;

namespace RemoteTest.Core.Interfaces
{
    public interface IMeterReadingValidator
    {
        List<MeterReadingDto> FilterValidReadValues(List<MeterReadingDto> readings);
        List<MeterReadingDto> FilterByAccountIds(List<MeterReadingDto> readings, HashSet<int> validAccountIds);
        List<MeterReadingDto> RemoveDuplicatesKeepLatest(List<MeterReadingDto> readings);
        List<MeterReadingDto> ExcludeOlderThanDb(List<MeterReadingDto> readings, Dictionary<int, MeterReading> dbLatestReadings);
    }
}
