using CadenceXP.Api.Models.Enums;

namespace CadenceXP.Api.Dtos;

public class UserQuestResponse
{
    public int QuestId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public QuestType Type { get; set; }
    public double TargetValue { get; set; }
    public int XpReward { get; set; }
    public double Progress { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
}