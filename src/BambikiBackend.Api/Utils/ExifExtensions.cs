using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace BambikiBackend.Api.Utils;

public static class ExifExtensions
{
    public static async Task<(double? Lat, double? Lng)> ExtractCoordinates(Image image)
    {
        var lat = image.Metadata.ExifProfile?.Values.FirstOrDefault(e => e.Tag == ExifTag.GPSLatitude)?.GetValue() as Rational[];
        var lng = image.Metadata.ExifProfile?.Values.FirstOrDefault(e => e.Tag == ExifTag.GPSLongitude)?.GetValue() as Rational[];

        if (lat is null || lng is null)
            return (null, null);

        var latConverted =
            GpsConverters.ConvertDegreeAngleToDouble(lat[0].ToDouble(), lat[1].ToDouble(), lat[2].ToDouble());
        var lngConverted =
            GpsConverters.ConvertDegreeAngleToDouble(lng[0].ToDouble(), lng[1].ToDouble(), lng[2].ToDouble());

        return (latConverted, lngConverted);
    }
}