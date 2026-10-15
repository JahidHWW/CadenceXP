using CadenceXP.Api.Models.Enums;

namespace CadenceXP.Api.Dtos;

public class QuestResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public QuestType Type { get; set; }
    public double TargetValue { get; set; }
    public int XpReward { get; set; }
}