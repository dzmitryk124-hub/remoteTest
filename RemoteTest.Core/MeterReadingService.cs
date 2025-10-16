using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Microsoft.EntityFrameworkCore;
using RemoteTest.Core.Dto;
using RemoteTest.Database;
using RemoteTest.Server.Entities;
using System.Globalization;
using System.Text.RegularExpressions;
using EFCore.BulkExtensions;

namespace RemoteTest.Core
{
    public sealed class MeterReadingService : IMeterReadingService
    {
        private readonly MeterReadingDbContext meterReadingDbContext;

        public MeterReadingService(MeterReadingDbContext meterReadingDbContext)
        {
            this.meterReadingDbContext = meterReadingDbContext;
        }

        public async Task<IEnumerable<MeterReadingViewDto>> GetMeterReadings()
        {
            var result = await this.meterReadingDbContext.MeterReadings
                .Select(x => new MeterReadingViewDto
                {
                    Id = x.Id,
                    AccountId = x.AccountId,
                    MeterReadingDateTime = x.MeterReadingDateTime,
                    MeterReadValue = x.MeterReadValue,
                    FirstName = x.Account.FirstName,
                    LastName = x.Account.LastName,
                }).ToListAsync();
            return result;
        }

        public async Task<MeterReadingsUploadResult> UploadMeterReadingsFile(Stream stream)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            // Step 1: Read MeterReadings from CSV
            List<MeterReadingDto> meterReadings = null;
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, config))
            {
                var options = new TypeConverterOptions { Formats = new[] { "dd/MM/yyyy HH:mm" } };
                csv.Context.TypeConverterOptionsCache.AddOptions<DateTime>(options);
                meterReadings = csv.GetRecords<MeterReadingDto>().ToList();

            }

            var meterReadingsTotal = meterReadings.Count();

            // Step 2: Filter MeterReadValue Format (NNNNN)
            var regex = new Regex(@"^\d{5}$");
            meterReadings = meterReadings
                .Where(r => regex.IsMatch(r.MeterReadValue ?? ""))
                .ToList();

            // Step 3: Filter by Associated AccountId in Database
            var dbAccountIds = this.meterReadingDbContext.Accounts.Select(a => a.AccountId).ToHashSet();
            meterReadings = meterReadings
                .Where(r => dbAccountIds.Contains(r.AccountId))
                .ToList();

            // Step 4: Remove Duplicates by AccountId, Keep Latest MeterReadingDateTime
            meterReadings = meterReadings
                .GroupBy(r => r.AccountId)
                .Select(g => g.OrderByDescending(r => r.MeterReadingDateTime).First())
                .ToList();

            // Step 5: Exclude Records with Older MeterReadingDateTime Than Latest in Database
            // Create a dictionary: AccountId -> MeterReadingDateTime
            var dbReadings = this.meterReadingDbContext.MeterReadings
                .GroupBy(r => r.AccountId)
                .Select(g => new { AccountId = g.Key, LatestDate = g.OrderBy(x => x.MeterReadingDateTime).Last() })
                .ToDictionary(x => x.AccountId, x => x.LatestDate);

            meterReadings = meterReadings
                .Where(r => !dbReadings.ContainsKey(r.AccountId) // No reading in DB for this AccountId
                         || r.MeterReadingDateTime > dbReadings[r.AccountId].MeterReadingDateTime) // Only keep strictly newer readings
                .ToList();

            // Step 6: Map to Entities to Insert or Update
            var filteredDbReadings = new List<MeterReading>();
            foreach (var meterReading in meterReadings)
            {
                var dbReading = dbReadings
                    .Where(x => x.Key == meterReading.AccountId)
                    .Select(x => x.Value)
                    .FirstOrDefault();
                dbReading ??= new MeterReading() 
                    {
                        AccountId = meterReading.AccountId,                        
                    };
                dbReading.MeterReadingDateTime = meterReading.MeterReadingDateTime;
                dbReading.MeterReadValue = meterReading.MeterReadValue;
                filteredDbReadings.Add(dbReading);
            }

            // Step 7: Bulk Insert or Update
            this.meterReadingDbContext.BulkInsertOrUpdate(filteredDbReadings, new BulkConfig
            {
                PreserveInsertOrder = true,
                SetOutputIdentity = true
            });

            await this.meterReadingDbContext.SaveChangesAsync();

            var successfulReadings = meterReadings.Count();
            var result = new MeterReadingsUploadResult
            {
                NumberOfSuccessfulReadings = successfulReadings,
                NumberOfFailedReadings = meterReadingsTotal - successfulReadings,
            };
            return result;

        }
    }
}
