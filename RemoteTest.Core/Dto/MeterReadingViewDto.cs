using CsvHelper.Configuration.Attributes;

namespace RemoteTest.Core.Dto
{
    public class MeterReadingViewDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        [Format("dd/MM/yyyy HH:mm")]
        public string MeterReadValue { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
