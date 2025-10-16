using RemoteTest.Core.Dto;
using RemoteTest.Core.Interfaces;
using RemoteTest.Server.Entities;
using System.Text.RegularExpressions;

namespace RemoteTest.Core.Services
{
    public class MeterReadingValidator : IMeterReadingValidator
    {
        private static readonly Regex MeterReadValueRegex = new(@"^\d{5}$");

        public List<MeterReadingDto> FilterValidReadValues(List<MeterReadingDto> readings)
            => readings.Where(r => MeterReadValueRegex.IsMatch(r.MeterReadValue ?? "")).ToList();

        public List<MeterReadingDto> FilterByAccountIds(List<MeterReadingDto> readings, HashSet<int> validAccountIds)
            => readings.Where(r => validAccountIds.Contains(r.AccountId)).ToList();

        public List<MeterReadingDto> RemoveDuplicatesKeepLatest(List<MeterReadingDto> readings)
            => readings.GroupBy(r => r.AccountId)
                       .Select(g => g.OrderByDescending(r => r.MeterReadingDateTime).First())
                       .ToList();

        public List<MeterReadingDto> ExcludeOlderThanDb(List<MeterReadingDto> readings, Dictionary<int, MeterReading> dbLatestReadings)
            => readings.Where(r => !dbLatestReadings.ContainsKey(r.AccountId) ||
                                  r.MeterReadingDateTime > dbLatestReadings[r.AccountId].MeterReadingDateTime)
                       .ToList();
    }
}
