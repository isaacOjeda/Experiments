namespace OpenTelemetry.Api.Data;

public class WeatherLog
{
    public int WeatherLogId { get; set; }
    public DateTime Date { get; set; }
    public string? RawData { get; set; }
}