namespace CadenceXP.Api.Models;

public class Ride
{
    public int Id {get; set;}
    public required string Name { get; set;}
    public int UserId {get; set;}
    public AppUser User {get; set;} = null!;
    public int BikeId {get; set;}
    public Bike Bike {get; set;} = null!;
    public double DistanceMeters {get; set;}
    public double ElevationGainMeters {get; set;}
    public int DurationSeconds {get; set;}
    public DateTime RideDateUtc {get; set;}
}