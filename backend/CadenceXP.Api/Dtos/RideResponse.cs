namespace CadenceXP.Api.Dtos;
using CadenceXP.Api.Models.Enums;

public class RideResponse
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public int UserId { get; set; }

    public required string UserDisplayName { get; set; }

    public int BikeId { get; set; }

    public required string BikeName { get; set; }

    public double DistanceMeters { get; set; }

    public double ElevationGainMeters { get; set; }

    public int DurationSeconds { get; set; }

    public DateTime RideDateUtc { get; set; }

    public string? OriginalFileName { get; set; }

    public RideProcessingStatus ProcessingStatus { get; set; }
}