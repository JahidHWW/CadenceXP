using CadenceXP.Api.Data;
using CadenceXP.Api.Models;
using CadenceXP.Api.Models.Enums;
using CadenceXP.Api.Services.Achievements;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CadenceXP.Api.Tests;

public class AchievementServiceTests
{
    private static async Task<(CadenceXpDbContext Database, SqliteConnection Connection)>
        CreateTestDatabaseAsync()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<CadenceXpDbContext>()
            .UseSqlite(connection)
            .Options;

        var database = new CadenceXpDbContext(options);

        await database.Database.EnsureCreatedAsync();

        return (database, connection);
    }

    private static async Task<(AppUser User, Bike Bike)>
        CreateTestUserAndBikeAsync(CadenceXpDbContext database)
    {
        var user = new AppUser
        {
            DisplayName = "Achievement Test User",
            Email = "achievement-test@example.com"
        };

        database.Users.Add(user);
        await database.SaveChangesAsync();

        var bike = new Bike
        {
            Name = "Achievement Test Bike",
            Type = "Road",
            UserId = user.Id
        };

        database.Bikes.Add(bike);
        await database.SaveChangesAsync();

        return (user, bike);
    }

    private static async Task<Ride> CreateCompletedRideAsync(
        CadenceXpDbContext database,
        AppUser user,
        Bike bike,
        double distanceMiles,
        double elevationMeters)
    {
        var ride = new Ride
        {
            Name = "Achievement Test Ride",
            UserId = user.Id,
            BikeId = bike.Id,
            DistanceMeters = distanceMiles * 1609.344,
            ElevationGainMeters = elevationMeters,
            DurationSeconds = 3600,
            XpEarned = 100,
            RideDateUtc = DateTime.UtcNow,
            ProcessingStatus = RideProcessingStatus.Completed
        };

        database.Rides.Add(ride);
        await database.SaveChangesAsync();

        return ride;
    }

    [Fact]
    public async Task UpdateUserAchievementsAsync_AwardsRetroactiveAchievements()
    {
        var (database, connection) = await CreateTestDatabaseAsync();

        await using (connection)
        await using (database)
        {
            var (user, bike) =
                await CreateTestUserAndBikeAsync(database);

            await CreateCompletedRideAsync(
                database,
                user,
                bike,
                distanceMiles: 6,
                elevationMeters: 600
            );

            await CreateCompletedRideAsync(
                database,
                user,
                bike,
                distanceMiles: 6,
                elevationMeters: 600
            );

            var service = new AchievementService(database);

            await service.UpdateUserAchievementsAsync(user.Id);

            var earnedAchievementIds = await database.UserAchievements
                .Where(userAchievement =>
                    userAchievement.UserId == user.Id)
                .Select(userAchievement =>
                    userAchievement.AchievementId)
                .OrderBy(id => id)
                .ToListAsync();

            Assert.Contains(1, earnedAchievementIds);
            Assert.Contains(4, earnedAchievementIds);
            Assert.Contains(7, earnedAchievementIds);

            Assert.DoesNotContain(2, earnedAchievementIds);
            Assert.DoesNotContain(10, earnedAchievementIds);
        }
    }

    [Fact]
    public async Task UpdateUserAchievementsAsync_WhenCalledTwice_DoesNotDuplicateAchievements()
    {
        var (database, connection) = await CreateTestDatabaseAsync();

        await using (connection)
        await using (database)
        {
            var (user, bike) =
                await CreateTestUserAndBikeAsync(database);

            await CreateCompletedRideAsync(
                database,
                user,
                bike,
                distanceMiles: 12,
                elevationMeters: 1200
            );

            var service = new AchievementService(database);

            await service.UpdateUserAchievementsAsync(user.Id);
            await service.UpdateUserAchievementsAsync(user.Id);

            var firstRideCount = await database.UserAchievements
                .CountAsync(userAchievement =>
                    userAchievement.UserId == user.Id &&
                    userAchievement.AchievementId == 1);

            var tenMileCount = await database.UserAchievements
                .CountAsync(userAchievement =>
                    userAchievement.UserId == user.Id &&
                    userAchievement.AchievementId == 4);

            var climberCount = await database.UserAchievements
                .CountAsync(userAchievement =>
                    userAchievement.UserId == user.Id &&
                    userAchievement.AchievementId == 7);

            var doubleDigitsCount = await database.UserAchievements
                .CountAsync(userAchievement =>
                    userAchievement.UserId == user.Id &&
                    userAchievement.AchievementId == 10);

            Assert.Equal(1, firstRideCount);
            Assert.Equal(1, tenMileCount);
            Assert.Equal(1, climberCount);
            Assert.Equal(1, doubleDigitsCount);
        }
    }

    [Fact]
    public async Task UpdateUserAchievementsAsync_SingleRideDistance_UsesLongestRide()
    {
        var (database, connection) = await CreateTestDatabaseAsync();

        await using (connection)
        await using (database)
        {
            var (user, bike) =
                await CreateTestUserAndBikeAsync(database);

            await CreateCompletedRideAsync(
                database,
                user,
                bike,
                distanceMiles: 6,
                elevationMeters: 0
            );

            await CreateCompletedRideAsync(
                database,
                user,
                bike,
                distanceMiles: 6,
                elevationMeters: 0
            );

            var service = new AchievementService(database);

            await service.UpdateUserAchievementsAsync(user.Id);

            var earnedAchievementIds = await database.UserAchievements
                .Where(userAchievement =>
                    userAchievement.UserId == user.Id)
                .Select(userAchievement =>
                    userAchievement.AchievementId)
                .ToListAsync();

            // 12 total miles means the cumulative 10-mile
            // achievement should be earned.
            Assert.Contains(4, earnedAchievementIds);

            // But neither individual ride was 10 miles,
            // so Double Digits must remain unearned.
            Assert.DoesNotContain(10, earnedAchievementIds);
        }
    }
}