namespace CadenceXP.Api.Dtos;

public class CreateRideRequest
{
    public required string Name { get; set; }

    public double DistanceMiles { get; set; }

    public DateTime RideDate { get; set; }
}