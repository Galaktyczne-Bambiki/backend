namespace BambikiBackend.Api.Models.FireReports;

public class FireReportModel
{
    public long FireReportId { get; set; }
    public string? Description { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
}