using System.Globalization;
using System.Xml.Linq;

namespace CadenceXP.Api.Services.Gpx;

public class GpxParser
{
    public List<GpxTrackPoint> Parse(string filePath)
    {
        var document = XDocument.Load(filePath);

        var trackPoints = document
            .Descendants()
            .Where(element => element.Name.LocalName == "trkpt")
            .Select(element => new GpxTrackPoint
            {
                Latitude = double.Parse(
                    element.Attribute("lat")!.Value,
                    CultureInfo.InvariantCulture
                ),

                Longitude = double.Parse(
                    element.Attribute("lon")!.Value,
                    CultureInfo.InvariantCulture
                ),

                ElevationMeters = double.Parse(
                    element.Elements()
                        .First(child => child.Name.LocalName == "ele")
                        .Value,
                    CultureInfo.InvariantCulture
                ),

                TimeUtc = DateTime.Parse(
                    element.Elements()
                        .First(child => child.Name.LocalName == "time")
                        .Value,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal
                )
            })
            .ToList();

        return trackPoints;
    }

    public double CalculateDistanceMeters(List<GpxTrackPoint> points)
    {
        double totalDistance = 0;

        for (int i = 1; i < points.Count; i++)
        {
            var previous = points[i - 1];
            var current = points[i];

            totalDistance += HaversineDistance(
                previous.Latitude,
                previous.Longitude,
                current.Latitude,
                current.Longitude
            );
        }

        return totalDistance;
    }

    private double HaversineDistance(
    double latitude1,
    double longitude1,
    double latitude2,
    double longitude2)
    {
        const double EarthRadiusMeters = 6371000;

        double lat1 = DegreesToRadians(latitude1);
        double lat2 = DegreesToRadians(latitude2);

        double deltaLat = DegreesToRadians(latitude2 - latitude1);
        double deltaLon = DegreesToRadians(longitude2 - longitude1);

        double a =
            Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
            Math.Cos(lat1) *
            Math.Cos(lat2) *
            Math.Sin(deltaLon / 2) *
            Math.Sin(deltaLon / 2);

        double c = 2 * Math.Atan2(
            Math.Sqrt(a),
            Math.Sqrt(1 - a)
        );

        return EarthRadiusMeters * c;
    }

    private double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }

    public double CalculateElevationGainMeters(List<GpxTrackPoint> points)
    {
        double totalGain = 0;

        for (int i = 1; i < points.Count; i++)
        {
            double difference =
                points[i].ElevationMeters -
                points[i - 1].ElevationMeters;

            if (difference > 0)
            {
                totalGain += difference;
            }
        }

        return totalGain;
    }

    public int CalculateDurationSeconds(List<GpxTrackPoint> points)
    {
        if (points.Count < 2)
        {
            return 0;
        }

        var duration =
            points[^1].TimeUtc -
            points[0].TimeUtc;

        return (int)duration.TotalSeconds;
    }
}