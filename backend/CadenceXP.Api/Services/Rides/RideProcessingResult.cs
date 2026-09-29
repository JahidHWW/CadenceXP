namespace CadenceXP.Api.Services.Rides;

public class RideProcessingResult
{
    public double DistanceMeters { get; set; }

    public double ElevationGainMeters { get; set; }

    public int DurationSeconds { get; set; }

    public DateTime RideDateUtc { get; set; }
}