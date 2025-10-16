using RemoteTest.Server.Entities;

namespace RemoteTest.Database.Repositories
{
    public interface IMeterReadingRepository
    {
        Task<HashSet<int>> GetAllAccountIdsAsync();
        Task<Dictionary<int, DateTime>> GetLatestReadingsByAccountIdAsync();
        Task BulkInsertOrUpdateAsync(List<MeterReading> readings);
        IQueryable<MeterReading> GetMeterReadingsQuery();
    }
}
