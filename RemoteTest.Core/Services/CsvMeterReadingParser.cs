using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using RemoteTest.Core.Dto;
using RemoteTest.Core.Interfaces;
using System.Globalization;

namespace RemoteTest.Core.Services
{
    public class CsvMeterReadingParser : ICsvMeterReadingParser
    {
        public List<MeterReadingDto> Parse(Stream stream)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, config);
            var options = new TypeConverterOptions { Formats = new[] { "dd/MM/yyyy HH:mm" } };
            csv.Context.TypeConverterOptionsCache.AddOptions<DateTime>(options);
            return csv.GetRecords<MeterReadingDto>().ToList();
        }
    }
}
