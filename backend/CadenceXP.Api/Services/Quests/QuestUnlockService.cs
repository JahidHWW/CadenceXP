using CadenceXP.Api.Data;
using CadenceXP.Api.Services.Xp;
using CadenceXP.Api.Models.Enums;
using CadenceXP.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CadenceXP.Api.Services.Quests;

public class QuestUnlockService
{
    private readonly CadenceXpDbContext _database;
    private readonly XpService _xpService;

    public QuestUnlockService(
        CadenceXpDbContext database,
        XpService xpService)
    {
        _database = database;
        _xpService = xpService;
    }

    public async Task UnlockEligibleQuestsAsync(int userId)
    {
        var rideXp = await _database.Rides
            .Where(ride => ride.UserId == userId
            && ride.ProcessingStatus == RideProcessingStatus.Completed)
            .SumAsync(ride => ride.XpEarned);

        var questXp = await _database.UserQuests
            .Where(userQuest => userQuest.UserId == userId
            && userQuest.IsCompleted)
            .SumAsync(userQuest => userQuest.Quest.XpReward);

        int totalXp = rideXp + questXp;

        int currentLevel = _xpService.CalculateLevel(totalXp);

        var eligibleQuests = await _database.Quests
            .Where(quest => quest.RequiredLevel <= currentLevel)
            .ToListAsync();

        var unlockedQuestIds = await _database.UserQuests
            .Where(userQuest => userQuest.UserId == userId)
            .Select(userQuest => userQuest.QuestId)
            .ToListAsync();

        var questsToUnlock = eligibleQuests
            .Where(quest => !unlockedQuestIds.Contains(quest.Id))
            .ToList();

        foreach (var quest in questsToUnlock)
        {
            var userQuest = new UserQuest
            {
                UserId = userId,
                QuestId = quest.Id,
                Progress = 0,
                IsCompleted = false,
                UnlockedAtUtc = DateTime.UtcNow,
                CompletedAtUtc = null
            };
            _database.UserQuests.Add(userQuest);
        }
        await _database.SaveChangesAsync();
    }
}