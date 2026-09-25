namespace CadenceXP.Api.Services.Gpx;

public class GpxTrackPoint
{
    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public double ElevationMeters { get; set; }

    public DateTime TimeUtc { get; set; }
}