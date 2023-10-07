namespace BambikiBackend.Api.Controllers.Models.TemperatureReports;

public class TemperatureReportResponse
{
    public DateTimeOffset Date { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double CelsiusValue { get; set; }
}