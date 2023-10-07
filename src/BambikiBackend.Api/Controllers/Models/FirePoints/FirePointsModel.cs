using System.Text.Json.Serialization;
using BambikiBackend.Api.Integrations.Firms.Models;

namespace BambikiBackend.Api.Controllers.Models.FirePoints;

public class FirePointsModel
{
    public FirePointsModel(AreaDataModel dataModel)
    {
        Latitude = dataModel.Latitude;
        Longitude = dataModel.Longitude;
        DateUtc = dataModel.AcqDate.ToDateTime(dataModel.AcqTime);
        Confidence = dataModel.Confidence switch
        {
            "l" => FireConfidence.Low,
            "n" => FireConfidence.Nominal,
            "h" => FireConfidence.High,
            _ => throw new ArgumentOutOfRangeException(nameof(dataModel.Confidence), $"Unknown value of confidence: {dataModel.Confidence}")
        };
    }
    
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public DateTime DateUtc { get; init; }
    public FireConfidence Confidence { get; init; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FireConfidence
{
    Low,
    Nominal,
    High
}