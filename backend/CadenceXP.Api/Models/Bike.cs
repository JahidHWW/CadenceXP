namespace CadenceXP.Api.Models;

public class Bike
{
    public int Id {get; set;}
    public required string Name {get; set;}
    public required string Type {get; set;}
    public int UserId {get; set;}
    public AppUser User {get; set;} = null!;
    public List<Ride> Rides {get; set;} = [];
}