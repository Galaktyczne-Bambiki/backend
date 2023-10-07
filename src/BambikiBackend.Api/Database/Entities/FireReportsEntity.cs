using System.ComponentModel.DataAnnotations;

namespace BambikiBackend.Api.Database.Entities;

public class FireReportsEntity
{
    [Key]
    public long FireReportId { get; set; }
    public string? Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public byte[] Image { get; set; }
}