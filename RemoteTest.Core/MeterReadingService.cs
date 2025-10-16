using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Microsoft.EntityFrameworkCore;
using RemoteTest.Core.Dto;
using RemoteTest.Database;
using System.Globalization;

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

        public async  Task<MeterReadingsUploadResult> UploadMeterReadingsFile(Stream stream)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, config))
            {
                var options = new TypeConverterOptions { Formats = new[] { "dd/MM/yyyy HH:mm" } };
                csv.Context.TypeConverterOptionsCache.AddOptions<DateTime>(options);
                var dtos = csv.GetRecords<MeterReadingDto>().ToList();

                var result = new MeterReadingsUploadResult
                {
                    NumberOfSuccessfulReadings = dtos.Count(),
                    NumberOfFailedReadings = 0,
                };
                return await Task.FromResult(result);
            }
        }
    }
}
