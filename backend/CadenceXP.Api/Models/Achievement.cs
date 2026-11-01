using CadenceXP.Api.Models.Enums;

namespace CadenceXP.Api.Models;

public class Achievement
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public AchievementType Type { get; set; }

    public double TargetValue { get; set; }

    public int XpReward { get; set; }
}