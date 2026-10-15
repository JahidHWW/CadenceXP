using CadenceXP.Api.Data;
using CadenceXP.Api.Models.Enums;
using CadenceXP.Api.Models;
using Microsoft.EntityFrameworkCore;
namespace CadenceXP.Api.Services.Quests;

public class QuestProgressService
{
    private readonly CadenceXpDbContext _database;

    public QuestProgressService(CadenceXpDbContext database)
    {
        _database = database;
    }

    public async Task UpdateUserQuestProgressAsync(int userId)
    {
        double totalDistanceMeters = await _database.Rides
            .Where(ride =>
            ride.UserId == userId &&
            ride.ProcessingStatus == RideProcessingStatus.Completed)
            .SumAsync(ride => ride.DistanceMeters);

        double totalElevationMeters = await _database.Rides
            .Where(ride => ride.UserId == userId &&
            ride.ProcessingStatus == RideProcessingStatus.Completed)
            .SumAsync(ride => ride.ElevationGainMeters);

        int rideCount = await _database.Rides
            .Where(ride => ride.UserId == userId &&
            ride.ProcessingStatus == RideProcessingStatus.Completed)
            .CountAsync();

        double totalDistanceMiles = totalDistanceMeters / 1609.344;

        var quests = await _database.Quests
            .ToListAsync();

        var userQuests = await _database.UserQuests
            .Where(userQuest => userQuest.UserId == userId)
            .ToListAsync();

        foreach (var quest in quests)
        {
            var userQuest = userQuests
                .FirstOrDefault(userQuest => userQuest.QuestId == quest.Id);
            double progressValue;
            switch (quest.Type)
            {
                case QuestType.Distance:
                    progressValue = totalDistanceMiles;
                    break;

                case QuestType.Elevation:
                    progressValue = totalElevationMeters;
                    break;

                case QuestType.RideCount:
                    progressValue = rideCount;
                    break;

                default:
                    continue;
            }
            if (userQuest == null)
            {
                userQuest = new UserQuest
                {
                    UserId = userId,
                    QuestId = quest.Id,
                    Progress = 0,
                    IsCompleted = false,
                    CompletedAtUtc = null
                };
                _database.UserQuests.Add(userQuest);
            }
            userQuest.Progress = Math.Min(
                progressValue,
                quest.TargetValue
            );
            if (!userQuest.IsCompleted &&
                progressValue >= quest.TargetValue)
            {
                userQuest.IsCompleted = true;
                userQuest.CompletedAtUtc = DateTime.UtcNow;
            }
        }
        await _database.SaveChangesAsync();
    }
}