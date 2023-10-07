using System.ComponentModel.DataAnnotations;

namespace BambikiBackend.Api.Database.Entities;

public class TemperatureReportsEntity
{
    [Key]
    public long TemperatureReportId { get; set; }
    public DateTimeOffset Date { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double CelsiusValue { get; set; }
}