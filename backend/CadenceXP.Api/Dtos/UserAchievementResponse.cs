using CadenceXP.Api.Models.Enums;

namespace CadenceXP.Api.Dtos;

public class UserAchievementResponse
{
    public int AchievementId { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public AchievementType Type { get; set; }

    public double TargetValue { get; set; }

    public int XpReward { get; set; }

    public bool IsEarned { get; set; }

    public DateTime? EarnedAtUtc { get; set; }

    public double Progress { get; set; }
}