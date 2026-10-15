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
                TargetValue = 5,
                XpReward = 75
            },

            new Quest
            {
                Id = 2,
                Name = "Climb 300 Meters",
                Description = "Climb a total of 300 meters.",
                Type = QuestType.Elevation,
                TargetValue = 300,
                XpReward = 100
            },

            new Quest
            {
                Id = 3,
                Name = "Complete 3 Rides",
                Description = "Complete your first 3 rides.",
                Type = QuestType.RideCount,
                TargetValue = 3,
                XpReward = 125
            }
        );
    }

}