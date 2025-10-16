using Microsoft.EntityFrameworkCore;
using RemoteTest.Core.Dto;
using RemoteTest.Core.Interfaces;
using RemoteTest.Database.Repositories;
using RemoteTest.Server.Entities;

public sealed class MeterReadingService : IMeterReadingService
{
    private readonly ICsvMeterReadingParser _csvParser;
    private readonly IMeterReadingValidator _validator;
    private readonly IMeterReadingRepository _repository;

    public MeterReadingService(
        ICsvMeterReadingParser csvParser,
        IMeterReadingValidator validator,
        IMeterReadingRepository repository)
    {
        _csvParser = csvParser;
        _validator = validator;
        _repository = repository;
    }

    public async Task<IEnumerable<MeterReadingViewDto>> GetMeterReadings()
        => await _repository.GetMeterReadingsQuery()
            .Select(x => new MeterReadingViewDto
            {
                Id = x.Id,
                AccountId = x.AccountId,
                MeterReadingDateTime = x.MeterReadingDateTime,
                MeterReadValue = x.MeterReadValue,
                FirstName = x.Account.FirstName,
                LastName = x.Account.LastName,
            }).ToListAsync();


    public async Task<MeterReadingsUploadResult> UploadMeterReadingsFile(Stream stream)
    {
        // Step 1: Parse CSV
        var meterReadings = _csvParser.Parse(stream);
        var totalReadings = meterReadings.Count;

        // Step 2: Filter valid MeterReadValue
        meterReadings = _validator.FilterValidReadValues(meterReadings);

        // Step 3: Filter by valid AccountIds
        var validAccountIds = await _repository.GetAllAccountIdsAsync();
        meterReadings = _validator.FilterByAccountIds(meterReadings, validAccountIds);

        // Step 4: Remove duplicates, keep latest
        meterReadings = _validator.RemoveDuplicatesKeepLatest(meterReadings);

        // Step 5: Exclude older than DB
        var dbLatestReadings = await _repository.GetLatestReadingsByAccountIdAsync();
        meterReadings = _validator.ExcludeOlderThanDb(meterReadings, dbLatestReadings);

        // Step 6: Map to entities

        var filteredDbReadings = new List<MeterReading>();
        foreach (var meterReading in meterReadings)
        {
            var dbReading = dbLatestReadings
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

        // Step 7: Bulk insert or update
        await _repository.BulkInsertOrUpdateAsync(filteredDbReadings);

        return new MeterReadingsUploadResult
        {
            NumberOfSuccessfulReadings = meterReadings.Count,
            NumberOfFailedReadings = totalReadings - meterReadings.Count
        };
    }
}