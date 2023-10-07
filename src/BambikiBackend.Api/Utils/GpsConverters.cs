namespace BambikiBackend.Api.Utils;

public static class GpsConverters
{
    public static double ConvertDegreeAngleToDouble( double degrees, double minutes, double seconds )
    {
        //Decimal degrees =
        //   whole number of degrees,
        //   plus minutes divided by 60,
        //   plus seconds divided by 3600

        return degrees + (minutes/60) + (seconds/3600);
    }
}