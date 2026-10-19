using CadenceXP.Api.Models.Enums;

namespace CadenceXP.Api.Models;

public class Quest
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public QuestType Type { get; set; }
    public int RequiredLevel { get; set; }
    public QuestDifficulty Difficulty { get; set; }
    public double TargetValue { get; set; }
    public int XpReward { get; set; }
}