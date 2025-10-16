using Microsoft.EntityFrameworkCore;
using RemoteTest.Server.Entities;
using EFCore.BulkExtensions;

namespace RemoteTest.Database.Repositories
{
    public class MeterReadingRepository : IMeterReadingRepository
    {
        private readonly MeterReadingDbContext _dbContext;

        public MeterReadingRepository(MeterReadingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HashSet<int>> GetAllAccountIdsAsync()
            => await _dbContext.Accounts.Select(a => a.AccountId).ToHashSetAsync();

        public async Task<Dictionary<int, DateTime>> GetLatestReadingsByAccountIdAsync()
            => await _dbContext.MeterReadings
                .GroupBy(r => r.AccountId)
                .Select(g => new
                {
                    g.Key,
                    LatestDate = g.Max(x => x.MeterReadingDateTime)
                })
                .ToDictionaryAsync(x => x.Key, x => x.LatestDate);

        public async Task BulkInsertOrUpdateAsync(List<MeterReading> readings)
        {
            await _dbContext.BulkInsertOrUpdateAsync(readings, new BulkConfig
            {
                PreserveInsertOrder = true,
                SetOutputIdentity = true
            });
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<MeterReading> GetMeterReadingsQuery()
            => _dbContext.MeterReadings.AsNoTracking().AsQueryable();
    }
}