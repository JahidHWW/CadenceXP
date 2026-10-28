using CadenceXP.Api.Models;
using CadenceXP.Api.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CadenceXP.Api.Data;

public class CadenceXpDbContext : DbContext
{
    public CadenceXpDbContext(DbContextOptions<CadenceXpDbContext> options)
        : base(options)
    {
    }

    public DbSet<AppUser> Users { get; set; } = null!;
    public DbSet<Bike> Bikes { get; set; } = null!;
    public DbSet<Ride> Rides { get; set; } = null!;
    public DbSet<Quest> Quests { get; set; } = null!;
    public DbSet<UserQuest> UserQuests { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserQuest>()
            .HasIndex(userQuest => new
            {
                userQuest.UserId,
                userQuest.QuestId
            })
            .IsUnique();

        modelBuilder.Entity<Quest>().HasData(
            new Quest
            {
                Id = 1,
                Name = "Ride 5 Miles",
                Description = "Ride a total of 5 miles.",
                Type = QuestType.Distance,
                RequiredLevel = 1,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 5,
                XpReward = 50
            },

            new Quest
            {
                Id = 2,
                Name = "Climb 300 Meters",
                Description = "Climb a total of 300 meters.",
                Type = QuestType.Elevation,
                RequiredLevel = 1,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 300,
                XpReward = 60
            },

            new Quest
            {
                Id = 3,
                Name = "Complete 3 Rides",
                Description = "Complete your first 3 rides.",
                Type = QuestType.RideCount,
                RequiredLevel = 1,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 3,
                XpReward = 75
            },

            new Quest
            {
                Id = 4,
                Name = "Ride 3 Miles in One Ride",
                Description = "Complete a single ride of at least 3 miles.",
                Type = QuestType.SingleRideDistance,
                RequiredLevel = 3,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 3,
                XpReward = 75
            },

            new Quest
            {
                Id = 5,
                Name = "Ride 15 Total Miles",
                Description = "Ride a total of 15 miles after unlocking this quest.",
                Type = QuestType.Distance,
                RequiredLevel = 5,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 15,
                XpReward = 100
            },

            new Quest
            {
                Id = 6,
                Name = "Climb 500 Meters",
                Description = "Climb a total of 500 meters after unlocking this quest.",
                Type = QuestType.Elevation,
                RequiredLevel = 5,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 500,
                XpReward = 100
            },

            new Quest
            {
                Id = 7,
                Name = "Complete 5 Rides",
                Description = "Complete 5 rides after unlocking this quest.",
                Type = QuestType.RideCount,
                RequiredLevel = 7,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 5,
                XpReward = 100
            },

            new Quest
            {
                Id = 8,
                Name = "Ride 5 Miles in One Ride",
                Description = "Complete a single ride of at least 5 miles.",
                Type = QuestType.SingleRideDistance,
                RequiredLevel = 7,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 5,
                XpReward = 125
            },

            new Quest
            {
                Id = 9,
                Name = "Climb 750 Meters",
                Description = "Climb a total of 750 meters after unlocking this quest.",
                Type = QuestType.Elevation,
                RequiredLevel = 8,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 750,
                XpReward = 125
            },

            new Quest
            {
                Id = 10,
                Name = "Ride 30 Total Miles",
                Description = "Ride a total of 30 miles after unlocking this quest.",
                Type = QuestType.Distance,
                RequiredLevel = 10,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 30,
                XpReward = 150
            },

            new Quest
            {
                Id = 11,
                Name = "Complete 10 Rides",
                Description = "Complete 10 rides after unlocking this quest.",
                Type = QuestType.RideCount,
                RequiredLevel = 10,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 10,
                XpReward = 150
            },

            new Quest
            {
                Id = 12,
                Name = "Ride 10 Miles in One Ride",
                Description = "Complete a single ride of at least 10 miles.",
                Type = QuestType.SingleRideDistance,
                RequiredLevel = 10,
                Difficulty = QuestDifficulty.Hard,
                TargetValue = 10,
                XpReward = 225
            },

            new Quest
            {
                Id = 13,
                Name = "Ride 50 Total Miles",
                Description = "Ride a total of 50 miles after unlocking this quest.",
                Type = QuestType.Distance,
                RequiredLevel = 12,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 50,
                XpReward = 200
            },

            new Quest
            {
                Id = 14,
                Name = "Complete 12 Rides",
                Description = "Complete 12 rides after unlocking this quest.",
                Type = QuestType.RideCount,
                RequiredLevel = 12,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 12,
                XpReward = 175
            },

            new Quest
            {
                Id = 15,
                Name = "Climb 1,000 Meters",
                Description = "Climb a total of 1,000 meters after unlocking this quest.",
                Type = QuestType.Elevation,
                RequiredLevel = 13,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 1000,
                XpReward = 200
            },

            new Quest
            {
                Id = 16,
                Name = "Ride 15 Miles in One Ride",
                Description = "Complete a single ride of at least 15 miles.",
                Type = QuestType.SingleRideDistance,
                RequiredLevel = 15,
                Difficulty = QuestDifficulty.Hard,
                TargetValue = 15,
                XpReward = 300
            },

            new Quest
            {
                Id = 17,
                Name = "Complete 15 Rides",
                Description = "Complete 15 rides after unlocking this quest.",
                Type = QuestType.RideCount,
                RequiredLevel = 15,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 15,
                XpReward = 200
            },

            new Quest
            {
                Id = 18,
                Name = "Ride 75 Total Miles",
                Description = "Ride a total of 75 miles after unlocking this quest.",
                Type = QuestType.Distance,
                RequiredLevel = 17,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 75,
                XpReward = 250
            },

            new Quest
            {
                Id = 19,
                Name = "Climb 1,500 Meters",
                Description = "Climb a total of 1,500 meters after unlocking this quest.",
                Type = QuestType.Elevation,
                RequiredLevel = 18,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 1500,
                XpReward = 250
            },

            new Quest
            {
                Id = 20,
                Name = "Complete 20 Rides",
                Description = "Complete 20 rides after unlocking this quest.",
                Type = QuestType.RideCount,
                RequiredLevel = 20,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 20,
                XpReward = 250
            },

            new Quest
            {
                Id = 21,
                Name = "Ride 20 Miles in One Ride",
                Description = "Complete a single ride of at least 20 miles.",
                Type = QuestType.SingleRideDistance,
                RequiredLevel = 20,
                Difficulty = QuestDifficulty.Hard,
                TargetValue = 20,
                XpReward = 350
            },

            new Quest
            {
                Id = 22,
                Name = "Ride 100 Total Miles",
                Description = "Ride a total of 100 miles after unlocking this quest.",
                Type = QuestType.Distance,
                RequiredLevel = 22,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 100,
                XpReward = 300
            },

            new Quest
            {
                Id = 23,
                Name = "Complete 25 Rides",
                Description = "Complete 25 rides after unlocking this quest.",
                Type = QuestType.RideCount,
                RequiredLevel = 22,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 25,
                XpReward = 250
            },

            new Quest
            {
                Id = 24,
                Name = "Climb 2,000 Meters",
                Description = "Climb a total of 2,000 meters after unlocking this quest.",
                Type = QuestType.Elevation,
                RequiredLevel = 23,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 2000,
                XpReward = 300
            },

            new Quest
            {
                Id = 25,
                Name = "Ride 25 Miles in One Ride",
                Description = "Complete a single ride of at least 25 miles.",
                Type = QuestType.SingleRideDistance,
                RequiredLevel = 25,
                Difficulty = QuestDifficulty.Hard,
                TargetValue = 25,
                XpReward = 400
            },

            new Quest
            {
                Id = 26,
                Name = "Complete 30 Rides",
                Description = "Complete 30 rides after unlocking this quest.",
                Type = QuestType.RideCount,
                RequiredLevel = 25,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 30,
                XpReward = 300
            },

            new Quest
            {
                Id = 27,
                Name = "Ride 150 Total Miles",
                Description = "Ride a total of 150 miles after unlocking this quest.",
                Type = QuestType.Distance,
                RequiredLevel = 27,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 150,
                XpReward = 350
            },

            new Quest
            {
                Id = 28,
                Name = "Climb 3,000 Meters",
                Description = "Climb a total of 3,000 meters after unlocking this quest.",
                Type = QuestType.Elevation,
                RequiredLevel = 28,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 3000,
                XpReward = 350
            },

            new Quest
            {
                Id = 29,
                Name = "Complete 40 Rides",
                Description = "Complete 40 rides after unlocking this quest.",
                Type = QuestType.RideCount,
                RequiredLevel = 30,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 40,
                XpReward = 350
            },

            new Quest
            {
                Id = 30,
                Name = "Ride 30 Miles in One Ride",
                Description = "Complete a single ride of at least 30 miles.",
                Type = QuestType.SingleRideDistance,
                RequiredLevel = 30,
                Difficulty = QuestDifficulty.Hard,
                TargetValue = 30,
                XpReward = 450
            },

            new Quest
            {
                Id = 31,
                Name = "Ride 200 Total Miles",
                Description = "Ride a total of 200 miles after unlocking this quest.",
                Type = QuestType.Distance,
                RequiredLevel = 32,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 200,
                XpReward = 400
            },

            new Quest
            {
                Id = 32,
                Name = "Complete 50 Rides",
                Description = "Complete 50 rides after unlocking this quest.",
                Type = QuestType.RideCount,
                RequiredLevel = 32,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 50,
                XpReward = 400
            },

            new Quest
            {
                Id = 33,
                Name = "Climb 4,000 Meters",
                Description = "Climb a total of 4,000 meters after unlocking this quest.",
                Type = QuestType.Elevation,
                RequiredLevel = 33,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 4000,
                XpReward = 400
            },

            new Quest
            {
                Id = 34,
                Name = "Ride 35 Miles in One Ride",
                Description = "Complete a single ride of at least 35 miles.",
                Type = QuestType.SingleRideDistance,
                RequiredLevel = 35,
                Difficulty = QuestDifficulty.Hard,
                TargetValue = 35,
                XpReward = 500
            },

            new Quest
            {
                Id = 35,
                Name = "Complete 60 Rides",
                Description = "Complete 60 rides after unlocking this quest.",
                Type = QuestType.RideCount,
                RequiredLevel = 35,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 60,
                XpReward = 400
            },

            new Quest
            {
                Id = 36,
                Name = "Ride 250 Total Miles",
                Description = "Ride a total of 250 miles after unlocking this quest.",
                Type = QuestType.Distance,
                RequiredLevel = 37,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 250,
                XpReward = 450
            },

            new Quest
            {
                Id = 37,
                Name = "Climb 5,000 Meters",
                Description = "Climb a total of 5,000 meters after unlocking this quest.",
                Type = QuestType.Elevation,
                RequiredLevel = 38,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 5000,
                XpReward = 450
            },

            new Quest
            {
                Id = 38,
                Name = "Complete 75 Rides",
                Description = "Complete 75 rides after unlocking this quest.",
                Type = QuestType.RideCount,
                RequiredLevel = 40,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 75,
                XpReward = 450
            },

            new Quest
            {
                Id = 39,
                Name = "Ride 40 Miles in One Ride",
                Description = "Complete a single ride of at least 40 miles.",
                Type = QuestType.SingleRideDistance,
                RequiredLevel = 40,
                Difficulty = QuestDifficulty.Hard,
                TargetValue = 40,
                XpReward = 600
            },

            new Quest
            {
                Id = 40,
                Name = "The Fifty",
                Description = "Complete a single ride of at least 50 miles.",
                Type = QuestType.SingleRideDistance,
                RequiredLevel = 40,
                Difficulty = QuestDifficulty.Elite,
                TargetValue = 50,
                XpReward = 900
            },

            new Quest
            {
                Id = 41,
                Name = "Ride 300 Total Miles",
                Description = "Ride a total of 300 miles after unlocking this quest.",
                Type = QuestType.Distance,
                RequiredLevel = 42,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 300,
                XpReward = 500
            },

            new Quest
            {
                Id = 42,
                Name = "Complete 90 Rides",
                Description = "Complete 90 rides after unlocking this quest.",
                Type = QuestType.RideCount,
                RequiredLevel = 42,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 90,
                XpReward = 500
            },

            new Quest
            {
                Id = 43,
                Name = "Climb 6,500 Meters",
                Description = "Climb a total of 6,500 meters after unlocking this quest.",
                Type = QuestType.Elevation,
                RequiredLevel = 43,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 6500,
                XpReward = 500
            },

            new Quest
            {
                Id = 44,
                Name = "Ride 45 Miles in One Ride",
                Description = "Complete a single ride of at least 45 miles.",
                Type = QuestType.SingleRideDistance,
                RequiredLevel = 45,
                Difficulty = QuestDifficulty.Hard,
                TargetValue = 45,
                XpReward = 650
            },

            new Quest
            {
                Id = 45,
                Name = "Complete 100 Rides",
                Description = "Complete 100 rides after unlocking this quest.",
                Type = QuestType.RideCount,
                RequiredLevel = 45,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 100,
                XpReward = 550
            },

            new Quest
            {
                Id = 46,
                Name = "Ride 400 Total Miles",
                Description = "Ride a total of 400 miles after unlocking this quest.",
                Type = QuestType.Distance,
                RequiredLevel = 47,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 400,
                XpReward = 600
            },

            new Quest
            {
                Id = 47,
                Name = "Climb 8,000 Meters",
                Description = "Climb a total of 8,000 meters after unlocking this quest.",
                Type = QuestType.Elevation,
                RequiredLevel = 48,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 8000,
                XpReward = 600
            },

            new Quest
            {
                Id = 48,
                Name = "Century Ride",
                Description = "Complete a single ride of at least 100 miles.",
                Type = QuestType.SingleRideDistance,
                RequiredLevel = 48,
                Difficulty = QuestDifficulty.Elite,
                TargetValue = 100,
                XpReward = 1500
            },

            new Quest
            {
                Id = 49,
                Name = "Complete 125 Rides",
                Description = "Complete 125 rides after unlocking this quest.",
                Type = QuestType.RideCount,
                RequiredLevel = 50,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 125,
                XpReward = 650
            },

            new Quest
            {
                Id = 50,
                Name = "Ride 500 Total Miles",
                Description = "Ride a total of 500 miles after unlocking this quest.",
                Type = QuestType.Distance,
                RequiredLevel = 50,
                Difficulty = QuestDifficulty.Standard,
                TargetValue = 500,
                XpReward = 700
            }
        );
    }
}