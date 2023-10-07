using System.Globalization;

namespace BambikiBackend.Api.Integrations.Firms.Models;

public class AreaDataModel
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public float BrightTi4 { get; init; }
    public float Scan { get; init; }
    public float Track { get; init; }
    public DateOnly AcqDate { get; init; }
    public TimeOnly AcqTime { get; init; }
    public string Satellite { get; init; }
    public string Instrument { get; init; }
    public string Confidence { get; init; }
    public string Version { get; init; }
    public double BrightTi5 { get; init; }
    public double Frp { get; init; }
    public string DayNight { get; init; }
    public AreaDataModel()
    {

    }

    public AreaDataModel(string csvLine)
    {
        var splitted = csvLine.Split(',');
        Latitude = double.Parse(splitted[0], CultureInfo.InvariantCulture);
        Longitude = double.Parse(splitted[1], CultureInfo.InvariantCulture);
        BrightTi4 = float.Parse(splitted[2], CultureInfo.InvariantCulture);
        Scan = float.Parse(splitted[3], CultureInfo.InvariantCulture);
        Track = float.Parse(splitted[4], CultureInfo.InvariantCulture);
        AcqDate = DateOnly.ParseExact(splitted[5], "yyyy-MM-dd");
        AcqTime = TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(long.Parse(splitted[6], CultureInfo.InvariantCulture)));
        Satellite = splitted[7];
        Instrument = splitted[8];
        Confidence = splitted[9];
        Version = splitted[10];
        BrightTi5 = double.Parse(splitted[11], CultureInfo.InvariantCulture);
        Frp = double.Parse(splitted[12], CultureInfo.InvariantCulture);
        DayNight = splitted[13];
    }
}