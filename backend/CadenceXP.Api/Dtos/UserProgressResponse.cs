namespace CadenceXP.Api.Dtos;

public class UserProgressResponse
{
    public int UserId { get; set; }
    public int TotalXp { get; set; }
    public int Level { get; set; }
    public int XpIntoLevel { get; set; }
    public int XpForNextLevel { get; set; }
}