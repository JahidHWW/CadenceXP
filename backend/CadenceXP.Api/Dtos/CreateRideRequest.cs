namespace CadenceXP.Api.Dtos;

public class CreateRideRequest
{
    public required string Name { get; set; }
    public int UserId {get; set;}
    public int BikeId {get; set;}
    public double DistanceMeters { get; set; }
    public double ElevationGainMeters {get; set;}
    public int DurationSeconds {get; set;}
    public DateTime RideDateUtc { get; set;}
}