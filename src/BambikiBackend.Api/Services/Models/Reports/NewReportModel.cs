namespace BambikiBackend.Api.Services.Models.Reports;

public class NewReportModel
{
    public string? Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public byte[] Image { get; set; }
}