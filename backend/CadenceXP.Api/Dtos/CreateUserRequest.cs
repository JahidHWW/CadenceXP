namespace CadenceXP.Api.Dtos;

public class CreateUserRequest
{
    public required string DisplayName {get; set;}
    public required string Email {get; set;}
}