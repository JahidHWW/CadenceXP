using CadenceXP.Api.Services.Gpx;

namespace CadenceXP.Api.Services.Rides;

public class RideProcessingService
{
    private readonly GpxParser _gpxParser;

    public RideProcessingService(GpxParser gpxParser)
    {
        _gpxParser = gpxParser;
    }

    public RideProcessingResult Process(string filePath)
    {
        var trackPoints = _gpxParser.Parse(filePath);

        if (trackPoints.Count < 2)
        {
            throw new InvalidOperationException(
                "The GPX file does not contain enough track points."
            );
        }

        double distanceMeters =
            _gpxParser.CalculateDistanceMeters(trackPoints);

        double elevationGainMeters =
            _gpxParser.CalculateElevationGainMeters(trackPoints);

        int durationSeconds =
            _gpxParser.CalculateDurationSeconds(trackPoints);

        DateTime rideDateUtc =
            trackPoints[0].TimeUtc;

        var rideResponse = new RideProcessingResult
        {
            DistanceMeters = distanceMeters,
            ElevationGainMeters = elevationGainMeters,
            DurationSeconds = durationSeconds,
            RideDateUtc = rideDateUtc
        };

        return rideResponse;
    }
}