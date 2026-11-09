using CadenceXP.Api.Data;
using CadenceXP.Api.Dtos;
using CadenceXP.Api.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CadenceXP.Api.Services.Streaks;

public class StreakService
{
    private const int WeeklyRideGoal = 2;

    private readonly CadenceXpDbContext _database;

    public StreakService(CadenceXpDbContext database)
    {
        _database = database;
    }

    public async Task<UserStreakResponse> GetUserStreakAsync(int userId)
    {
        var nowUtc = DateTime.UtcNow;

        var rideDates = await _database.Rides
            .Where(ride =>
                ride.UserId == userId &&
                ride.ProcessingStatus == RideProcessingStatus.Completed &&
                ride.RideDateUtc <= nowUtc)
            .Select(ride => ride.RideDateUtc)
            .ToListAsync();

        var currentWeekStart = GetWeekStartUtc(nowUtc);

        var ridesByWeek = rideDates
            .GroupBy(GetWeekStartUtc)
            .ToDictionary(
                group => group.Key,
                group => group.Count()
            );

        int currentWeekRideCount =
            ridesByWeek.GetValueOrDefault(currentWeekStart, 0);

        var currentWeekRideDays = rideDates
            .Where(rideDate =>
                GetWeekStartUtc(rideDate) == currentWeekStart)
            .Select(rideDate =>
                ((int)rideDate.DayOfWeek + 6) % 7)
            .Distinct()
            .OrderBy(day => day)
            .ToList();

        var qualifyingWeeks = ridesByWeek
            .Where(week => week.Value >= WeeklyRideGoal)
            .Select(week => week.Key)
            .OrderBy(week => week)
            .ToList();

        int longestStreak = CalculateLongestStreak(
            qualifyingWeeks
        );

        int currentStreak = CalculateCurrentStreak(
            ridesByWeek,
            currentWeekStart,
            currentWeekRideCount
        );

        return new UserStreakResponse
        {
            CurrentStreak = currentStreak,
            LongestStreak = longestStreak,
            WeeklyRideGoal = WeeklyRideGoal,
            CurrentWeekRideCount = currentWeekRideCount,
            CurrentWeekGoalMet =
                currentWeekRideCount >= WeeklyRideGoal,
            CurrentWeekRideDays = currentWeekRideDays
        };
    }

    private static int CalculateCurrentStreak(
        Dictionary<DateTime, int> ridesByWeek,
        DateTime currentWeekStart,
        int currentWeekRideCount)
    {
        var weekToCheck =
            currentWeekRideCount >= WeeklyRideGoal
                ? currentWeekStart
                : currentWeekStart.AddDays(-7);

        int streak = 0;

        while (
            ridesByWeek.TryGetValue(
                weekToCheck,
                out int rideCount
            ) &&
            rideCount >= WeeklyRideGoal
        )
        {
            streak++;
            weekToCheck = weekToCheck.AddDays(-7);
        }

        return streak;
    }

    private static int CalculateLongestStreak(
        List<DateTime> qualifyingWeeks)
    {
        if (qualifyingWeeks.Count == 0)
        {
            return 0;
        }

        int longestStreak = 1;
        int currentStreak = 1;

        for (int i = 1; i < qualifyingWeeks.Count; i++)
        {
            if (
                qualifyingWeeks[i] ==
                qualifyingWeeks[i - 1].AddDays(7)
            )
            {
                currentStreak++;
            }
            else
            {
                currentStreak = 1;
            }

            longestStreak = Math.Max(
                longestStreak,
                currentStreak
            );
        }

        return longestStreak;
    }

    private static DateTime GetWeekStartUtc(
        DateTime dateTime)
    {
        var utcDate = dateTime.ToUniversalTime();

        int daysSinceMonday =
            ((int)utcDate.DayOfWeek + 6) % 7;

        return utcDate.Date.AddDays(
            -daysSinceMonday
        );
    }
}