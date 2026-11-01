using CadenceXP.Api.Data;
using CadenceXP.Api.Models;
using CadenceXP.Api.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CadenceXP.Api.Services.Achievements;

public class AchievementService
{
    private readonly CadenceXpDbContext _database;

    public AchievementService(CadenceXpDbContext database)
    {
        _database = database;
    }

    public async Task UpdateUserAchievementsAsync(int userId)
    {
        var completedRides = await _database.Rides
            .Where(ride =>
                ride.UserId == userId &&
                ride.ProcessingStatus == RideProcessingStatus.Completed)
            .ToListAsync();

        if (completedRides.Count == 0)
        {
            return;
        }

        double totalDistanceMiles =
            completedRides.Sum(ride => ride.DistanceMeters) / 1609.344;

        double totalElevationMeters =
            completedRides.Sum(ride => ride.ElevationGainMeters);

        int rideCount = completedRides.Count;

        double longestRideMiles = completedRides
            .Select(ride => ride.DistanceMeters / 1609.344)
            .DefaultIfEmpty(0)
            .Max();

        var achievements = await _database.Achievements
            .ToListAsync();

        var earnedAchievementIds = await _database.UserAchievements
            .Where(userAchievement => userAchievement.UserId == userId)
            .Select(userAchievement => userAchievement.AchievementId)
            .ToListAsync();

        foreach (var achievement in achievements)
        {
            if (earnedAchievementIds.Contains(achievement.Id))
            {
                continue;
            }

            double currentValue;

            switch (achievement.Type)
            {
                case AchievementType.TotalDistance:
                    currentValue = totalDistanceMiles;
                    break;

                case AchievementType.TotalElevation:
                    currentValue = totalElevationMeters;
                    break;

                case AchievementType.RideCount:
                    currentValue = rideCount;
                    break;

                case AchievementType.SingleRideDistance:
                    currentValue = longestRideMiles;
                    break;

                default:
                    continue;
            }

            if (currentValue < achievement.TargetValue)
            {
                continue;
            }

            var userAchievement = new UserAchievement
            {
                UserId = userId,
                AchievementId = achievement.Id,
                EarnedAtUtc = DateTime.UtcNow
            };

            _database.UserAchievements.Add(userAchievement);
        }

        await _database.SaveChangesAsync();
    }
}