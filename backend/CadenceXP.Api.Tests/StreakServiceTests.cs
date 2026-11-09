using CadenceXP.Api.Data;
using CadenceXP.Api.Models;
using CadenceXP.Api.Models.Enums;
using CadenceXP.Api.Services.Streaks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CadenceXP.Api.Tests;

public class StreakServiceTests
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
            DisplayName = "Streak Test User",
            Email = "streak-test@example.com"
        };

        database.Users.Add(user);
        await database.SaveChangesAsync();

        var bike = new Bike
        {
            Name = "Streak Test Bike",
            Type = "Road",
            UserId = user.Id
        };

        database.Bikes.Add(bike);
        await database.SaveChangesAsync();

        return (user, bike);
    }

    private static async Task CreateRideAsync(
        CadenceXpDbContext database,
        AppUser user,
        Bike bike,
        DateTime rideDateUtc,
        RideProcessingStatus status = RideProcessingStatus.Completed)
    {
        var ride = new Ride
        {
            Name = "Streak Test Ride",
            UserId = user.Id,
            BikeId = bike.Id,
            DistanceMeters = 5000,
            ElevationGainMeters = 100,
            DurationSeconds = 1800,
            XpEarned = 50,
            RideDateUtc = rideDateUtc,
            ProcessingStatus = status
        };

        database.Rides.Add(ride);
        await database.SaveChangesAsync();
    }

    private static DateTime GetCurrentWeekStartUtc()
    {
        var nowUtc = DateTime.UtcNow;

        int daysSinceMonday =
            ((int)nowUtc.DayOfWeek + 6) % 7;

        return nowUtc.Date.AddDays(-daysSinceMonday);
    }

    [Fact]
    public async Task GetUserStreakAsync_WhenCurrentAndPreviousWeeksQualify_ReturnsTwoWeekStreak()
    {
        var (database, connection) = await CreateTestDatabaseAsync();

        await using (connection)
        await using (database)
        {
            var (user, bike) =
                await CreateTestUserAndBikeAsync(database);

            var currentWeek = GetCurrentWeekStartUtc();
            var previousWeek = currentWeek.AddDays(-7);

            await CreateRideAsync(
                database,
                user,
                bike,
                previousWeek.AddDays(1).AddHours(12)
            );

            await CreateRideAsync(
                database,
                user,
                bike,
                previousWeek.AddDays(3).AddHours(12)
            );

            await CreateRideAsync(
                database,
                user,
                bike,
                currentWeek.AddDays(1).AddHours(12)
            );

            await CreateRideAsync(
                database,
                user,
                bike,
                currentWeek.AddDays(2).AddHours(12)
            );

            var service = new StreakService(database);

            var result =
                await service.GetUserStreakAsync(user.Id);

            Assert.Equal(2, result.CurrentStreak);
            Assert.Equal(2, result.LongestStreak);
            Assert.Equal(2, result.WeeklyRideGoal);
            Assert.Equal(2, result.CurrentWeekRideCount);
            Assert.True(result.CurrentWeekGoalMet);
        }
    }

    [Fact]
    public async Task GetUserStreakAsync_WhenCurrentWeekIsIncomplete_PreservesPreviousStreak()
    {
        var (database, connection) = await CreateTestDatabaseAsync();

        await using (connection)
        await using (database)
        {
            var (user, bike) =
                await CreateTestUserAndBikeAsync(database);

            var currentWeek = GetCurrentWeekStartUtc();

            var previousWeek = currentWeek.AddDays(-7);
            var twoWeeksAgo = currentWeek.AddDays(-14);

            await CreateRideAsync(
                database,
                user,
                bike,
                twoWeeksAgo.AddDays(1).AddHours(12)
            );

            await CreateRideAsync(
                database,
                user,
                bike,
                twoWeeksAgo.AddDays(3).AddHours(12)
            );

            await CreateRideAsync(
                database,
                user,
                bike,
                previousWeek.AddDays(1).AddHours(12)
            );

            await CreateRideAsync(
                database,
                user,
                bike,
                previousWeek.AddDays(3).AddHours(12)
            );

            // Only one ride this week.
            // The week is still in progress, so the old streak survives.
            await CreateRideAsync(
                database,
                user,
                bike,
                currentWeek.AddHours(12)
            );

            var service = new StreakService(database);

            var result =
                await service.GetUserStreakAsync(user.Id);

            Assert.Equal(2, result.CurrentStreak);
            Assert.Equal(2, result.LongestStreak);
            Assert.Equal(1, result.CurrentWeekRideCount);
            Assert.False(result.CurrentWeekGoalMet);
        }
    }

    [Fact]
    public async Task GetUserStreakAsync_WhenACompletedWeekIsMissed_BreaksCurrentStreak()
    {
        var (database, connection) = await CreateTestDatabaseAsync();

        await using (connection)
        await using (database)
        {
            var (user, bike) =
                await CreateTestUserAndBikeAsync(database);

            var currentWeek = GetCurrentWeekStartUtc();

            var previousWeek = currentWeek.AddDays(-7);
            var twoWeeksAgo = currentWeek.AddDays(-14);

            // Two weeks ago qualifies.
            await CreateRideAsync(
                database,
                user,
                bike,
                twoWeeksAgo.AddDays(1).AddHours(12)
            );

            await CreateRideAsync(
                database,
                user,
                bike,
                twoWeeksAgo.AddDays(3).AddHours(12)
            );

            // Previous week has only one ride, so the streak broke.
            await CreateRideAsync(
                database,
                user,
                bike,
                previousWeek.AddDays(2).AddHours(12)
            );

            var service = new StreakService(database);

            var result =
                await service.GetUserStreakAsync(user.Id);

            Assert.Equal(0, result.CurrentStreak);
            Assert.Equal(1, result.LongestStreak);
        }
    }

    [Fact]
    public async Task GetUserStreakAsync_IgnoresNonCompletedRides()
    {
        var (database, connection) = await CreateTestDatabaseAsync();

        await using (connection)
        await using (database)
        {
            var (user, bike) =
                await CreateTestUserAndBikeAsync(database);

            var currentWeek = GetCurrentWeekStartUtc();

            await CreateRideAsync(
                database,
                user,
                bike,
                currentWeek.AddHours(12),
                RideProcessingStatus.Completed
            );

            await CreateRideAsync(
                database,
                user,
                bike,
                currentWeek.AddDays(1).AddHours(12),
                RideProcessingStatus.Failed
            );

            var service = new StreakService(database);

            var result =
                await service.GetUserStreakAsync(user.Id);

            Assert.Equal(1, result.CurrentWeekRideCount);
            Assert.False(result.CurrentWeekGoalMet);
            Assert.Equal(0, result.CurrentStreak);
        }
    }
}