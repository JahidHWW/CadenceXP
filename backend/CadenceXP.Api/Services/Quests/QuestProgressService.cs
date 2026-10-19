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
        var completedRides = await _database.Rides
        .Where(ride =>
            ride.UserId == userId &&
            ride.ProcessingStatus == RideProcessingStatus.Completed)
        .ToListAsync();

        var userQuests = await _database.UserQuests
            .Where(userQuest => userQuest.UserId == userId)
            .Include(userQuest => userQuest.Quest)
            .ToListAsync();

        foreach (var userQuest in userQuests)
        {
            var quest = userQuest.Quest;
            var ridesSinceUnlock = completedRides
                .Where(ride => ride.RideDateUtc >= userQuest.UnlockedAtUtc)
                .ToList();

            double distanceMeters = ridesSinceUnlock
                .Sum(ride => ride.DistanceMeters);
            double elevationMeters = ridesSinceUnlock
                .Sum(ride => ride.ElevationGainMeters);
            int rideCount = ridesSinceUnlock.Count;
            double distanceMiles = distanceMeters / 1609.344;
            double progressValue;
            switch (quest.Type)
            {
                case QuestType.Distance:
                    progressValue = distanceMiles;
                    break;

                case QuestType.Elevation:
                    progressValue = elevationMeters;
                    break;

                case QuestType.RideCount:
                    progressValue = rideCount;
                    break;

                default:
                    continue;
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