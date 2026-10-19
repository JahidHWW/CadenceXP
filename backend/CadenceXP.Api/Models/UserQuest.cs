namespace CadenceXP.Api.Models;

public class UserQuest
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;

    public int QuestId { get; set; }
    public Quest Quest { get; set; } = null!;

    public double Progress { get; set; }
    public bool IsCompleted { get; set; }

    public DateTime UnlockedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
}