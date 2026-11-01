namespace CadenceXP.Api.Models;

public class UserAchievement
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public AppUser User { get; set; } = null!;

    public int AchievementId { get; set; }

    public Achievement Achievement { get; set; } = null!;

    public DateTime EarnedAtUtc { get; set; }
}