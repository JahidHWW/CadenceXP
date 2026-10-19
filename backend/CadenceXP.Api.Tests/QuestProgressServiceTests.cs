using CadenceXP.Api.Data;
using CadenceXP.Api.Services.Quests;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using CadenceXP.Api.Models;
using CadenceXP.Api.Models.Enums;

public class QuestProgressServiceTests
{
    [Fact]
    public void TestDatabase_CreatesSeededQuests()
    {
        using var connection =
            new SqliteConnection("DataSource=:memory:");

        connection.Open();

        using var database = CreateTestDatabase(connection);

        int questCount = database.Quests.Count();

        Assert.Equal(3, questCount);
    }

    [Fact]
    public async Task UpdateUserQuestProgressAsync_WithOneMileRide_UpdatesDistanceProgress()
    {
        using var connection =
            new SqliteConnection("DataSource=:memory:");

        connection.Open();

        using var database = CreateTestDatabase(connection);

        var (user, bike) =
            await CreateTestUserAndBikeAsync(database);

        await CreateUnlockedQuestAsync(database, user, 1);

        await CreateTestRideAsync(
            database,
            user,
            bike,
            1609.344,
            0,
            RideProcessingStatus.Completed
        );

        var service = new QuestProgressService(database);
        await service.UpdateUserQuestProgressAsync(user.Id);

        var distanceQuest = await database.UserQuests
            .Include(userQuest => userQuest.Quest)
            .SingleAsync(userQuest =>
                userQuest.UserId == user.Id &&
                userQuest.Quest.Type == QuestType.Distance);

        Assert.Equal(1.0, distanceQuest.Progress, 6);
        Assert.False(distanceQuest.IsCompleted);
        Assert.Null(distanceQuest.CompletedAtUtc);
    }

    [Fact]
    public async Task UpdateUserQuestProgressAsync_WhenTargetReached_CompletesDistanceQuest()
    {
        using var connection =
            new SqliteConnection("DataSource=:memory:");

        connection.Open();

        using var database = CreateTestDatabase(connection);

        var (user, bike) =
            await CreateTestUserAndBikeAsync(database);

        await CreateUnlockedQuestAsync(database, user, 1);

        await CreateTestRideAsync(
            database,
            user,
            bike,
            1609.344 * 6,
            0,
            RideProcessingStatus.Completed
        );

        var service = new QuestProgressService(database);
        await service.UpdateUserQuestProgressAsync(user.Id);

        var distanceQuest = await database.UserQuests
            .Include(userQuest => userQuest.Quest)
            .SingleAsync(userQuest =>
                userQuest.UserId == user.Id &&
                userQuest.Quest.Type == QuestType.Distance);

        Assert.Equal(5.0, distanceQuest.Progress, 6);
        Assert.True(distanceQuest.IsCompleted);
        Assert.NotNull(distanceQuest.CompletedAtUtc);
    }

    [Fact]
    public async Task UpdateUserQuestProgressAsync_WhenCalledTwice_DoesNotDoubleCountProgress()
    {
        using var connection =
            new SqliteConnection("DataSource=:memory:");

        connection.Open();

        using var database = CreateTestDatabase(connection);

        var (user, bike) =
            await CreateTestUserAndBikeAsync(database);

        await CreateUnlockedQuestAsync(database, user, 1);

        await CreateTestRideAsync(
            database,
            user,
            bike,
            1609.344 * 6,
            0,
            RideProcessingStatus.Completed
        );

        var service = new QuestProgressService(database);
        await service.UpdateUserQuestProgressAsync(user.Id);

        var firstResult = await database.UserQuests
            .Include(userQuest => userQuest.Quest)
            .SingleAsync(userQuest =>
                userQuest.UserId == user.Id &&
                userQuest.Quest.Type == QuestType.Distance);

        DateTime? firstCompletedAtUtc = firstResult.CompletedAtUtc;
        await service.UpdateUserQuestProgressAsync(user.Id);

        var secondResult = await database.UserQuests
            .Include(userQuest => userQuest.Quest)
            .SingleAsync(userQuest =>
                userQuest.UserId == user.Id &&
                userQuest.Quest.Type == QuestType.Distance);

        Assert.Equal(5.0, secondResult.Progress, 6);
        Assert.True(secondResult.IsCompleted);
        Assert.Equal(firstCompletedAtUtc, secondResult.CompletedAtUtc);
    }

    [Fact]
    public async Task UpdateUserQuestProgressAsync_WithPendingRide_DoesNotUpdateProgress()
    {
        using var connection =
            new SqliteConnection("DataSource=:memory:");

        connection.Open();

        using var database = CreateTestDatabase(connection);

        var (user, bike) =
            await CreateTestUserAndBikeAsync(database);

        await CreateUnlockedQuestAsync(database, user, 1);

        await CreateTestRideAsync(
            database,
            user,
            bike,
            1609.344 * 6,
            0,
            RideProcessingStatus.Pending
        );

        var service = new QuestProgressService(database);

        await service.UpdateUserQuestProgressAsync(user.Id);

        var distanceQuest = await database.UserQuests
            .Include(userQuest => userQuest.Quest)
            .SingleAsync(userQuest =>
                userQuest.UserId == user.Id &&
                userQuest.Quest.Type == QuestType.Distance);

        Assert.Equal(0.0, distanceQuest.Progress, 6);
        Assert.False(distanceQuest.IsCompleted);
        Assert.Null(distanceQuest.CompletedAtUtc);
    }

    [Fact]
    public async Task UpdateUserQuestProgressAsync_WithThreeCompletedRides_CompletesRideCountQuest()
    {
        using var connection =
            new SqliteConnection("DataSource=:memory:");

        connection.Open();

        using var database = CreateTestDatabase(connection);

        var (user, bike) =
            await CreateTestUserAndBikeAsync(database);

        await CreateUnlockedQuestAsync(database, user, 3);

        await CreateTestRideAsync(
            database,
            user,
            bike,
            100,
            0,
            RideProcessingStatus.Completed
        );

        await CreateTestRideAsync(
            database,
            user,
            bike,
            100,
            0,
            RideProcessingStatus.Completed
        );

        await CreateTestRideAsync(
            database,
            user,
            bike,
            100,
            0,
            RideProcessingStatus.Completed
        );

        var service = new QuestProgressService(database);

        await service.UpdateUserQuestProgressAsync(user.Id);

        var rideCountQuest = await database.UserQuests
            .Include(userQuest => userQuest.Quest)
            .SingleAsync(userQuest =>
                userQuest.UserId == user.Id &&
                userQuest.Quest.Type == QuestType.RideCount);

        Assert.Equal(3.0, rideCountQuest.Progress, 6);
        Assert.True(rideCountQuest.IsCompleted);
        Assert.NotNull(rideCountQuest.CompletedAtUtc);
    }

    [Fact]
    public async Task UpdateUserQuestProgressAsync_With300MetersElevation_CompletesElevationQuest()
    {
        using var connection =
            new SqliteConnection("DataSource=:memory:");

        connection.Open();

        using var database = CreateTestDatabase(connection);

        var (user, bike) =
            await CreateTestUserAndBikeAsync(database);

        await CreateUnlockedQuestAsync(database, user, 2);

        await CreateTestRideAsync(
            database,
            user,
            bike,
            100,
            300,
            RideProcessingStatus.Completed
        );

        var service = new QuestProgressService(database);

        await service.UpdateUserQuestProgressAsync(user.Id);

        var elevationQuest = await database.UserQuests
            .Include(userQuest => userQuest.Quest)
            .SingleAsync(userQuest =>
                userQuest.UserId == user.Id &&
                userQuest.Quest.Type == QuestType.Elevation);

        Assert.Equal(300.0, elevationQuest.Progress, 6);
        Assert.True(elevationQuest.IsCompleted);
        Assert.NotNull(elevationQuest.CompletedAtUtc);
    }

    [Fact]
    public async Task UpdateUserQuestProgressAsync_DoesNotCountAnotherUsersRides()
    {
        using var connection =
            new SqliteConnection("DataSource=:memory:");

        connection.Open();

        using var database = CreateTestDatabase(connection);

        var (userOne, bikeOne) =
            await CreateTestUserAndBikeAsync(database);

        await CreateUnlockedQuestAsync(database, userOne, 1);

        var userTwo = new AppUser
        {
            DisplayName = "Second User",
            Email = "second@example.com"
        };

        database.Users.Add(userTwo);
        await database.SaveChangesAsync();

        var bikeTwo = new Bike
        {
            Name = "Second Bike",
            Type = "Road",
            UserId = userTwo.Id
        };

        database.Bikes.Add(bikeTwo);
        await database.SaveChangesAsync();

        await CreateTestRideAsync(
            database,
            userTwo,
            bikeTwo,
            1609.344 * 6,
            0,
            RideProcessingStatus.Completed
        );

        var service = new QuestProgressService(database);

        await service.UpdateUserQuestProgressAsync(userOne.Id);

        var distanceQuest = await database.UserQuests
            .Include(userQuest => userQuest.Quest)
            .SingleAsync(userQuest =>
                userQuest.UserId == userOne.Id &&
                userQuest.Quest.Type == QuestType.Distance);

        Assert.Equal(0.0, distanceQuest.Progress, 6);
        Assert.False(distanceQuest.IsCompleted);
        Assert.Null(distanceQuest.CompletedAtUtc);
    }

    private static CadenceXpDbContext CreateTestDatabase(
    SqliteConnection connection)
    {
        var options =
            new DbContextOptionsBuilder<CadenceXpDbContext>()
                .UseSqlite(connection)
                .Options;

        var database =
            new CadenceXpDbContext(options);

        database.Database.EnsureCreated();

        return database;
    }

    private static async Task<(AppUser User, Bike Bike)> CreateTestUserAndBikeAsync(
    CadenceXpDbContext database)
    {
        var user = new AppUser
        {
            DisplayName = "Test User",
            Email = "test@example.com"
        };

        database.Users.Add(user);
        await database.SaveChangesAsync();

        var bike = new Bike
        {
            Name = "Test Bike",
            Type = "Road",
            UserId = user.Id
        };

        database.Bikes.Add(bike);
        await database.SaveChangesAsync();

        return (user, bike);
    }

    private static async Task<Ride> CreateTestRideAsync(
    CadenceXpDbContext database,
    AppUser user,
    Bike bike,
    double distanceMeters,
    double elevationGainMeters,
    RideProcessingStatus processingStatus)
    {
        var ride = new Ride
        {
            Name = "Test Ride",
            UserId = user.Id,
            BikeId = bike.Id,
            DistanceMeters = distanceMeters,
            ElevationGainMeters = elevationGainMeters,
            DurationSeconds = 600,
            XpEarned = 0,
            RideDateUtc = DateTime.UtcNow,
            ProcessingStatus = processingStatus
        };

        database.Rides.Add(ride);

        await database.SaveChangesAsync();

        return ride;
    }

    private static async Task<UserQuest> CreateUnlockedQuestAsync(
    CadenceXpDbContext database,
    AppUser user,
    int questId)
    {
        var userQuest = new UserQuest
        {
            UserId = user.Id,
            QuestId = questId,
            Progress = 0,
            IsCompleted = false,
            UnlockedAtUtc = DateTime.UtcNow.AddMinutes(-1),
            CompletedAtUtc = null
        };

        database.UserQuests.Add(userQuest);
        await database.SaveChangesAsync();

        return userQuest;
    }
}