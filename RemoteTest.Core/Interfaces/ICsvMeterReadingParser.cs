using RemoteTest.Core.Dto;

namespace RemoteTest.Core.Interfaces
{
    public interface ICsvMeterReadingParser
    {
        List<MeterReadingDto> Parse(Stream stream);
    }
}
