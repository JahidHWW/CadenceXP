namespace CadenceXP.Api.Models;

public class Ride
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public double DistanceMiles { get; set; }

    public DateTime RideDate { get; set; }
}