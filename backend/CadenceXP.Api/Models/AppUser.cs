namespace CadenceXP.Api.Models;

public class AppUser
{
    public int Id {get; set;}
    public required string DisplayName {get; set;}
    public required string Email {get; set;}
    public List<Bike> Bikes {get; set;} = [];
    public List<Ride> Rides {get; set;} = [];
}