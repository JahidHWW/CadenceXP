namespace CadenceXP.Api.Dtos;

public class CreateBikeRequest
{
    public required string Name {get; set;}
    public required string Type {get; set;}
    public int UserId {get; set;}
}