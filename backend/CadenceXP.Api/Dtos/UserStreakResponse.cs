namespace CadenceXP.Api.Dtos;

public class UserStreakResponse
{
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public int WeeklyRideGoal { get; set; }
    public int CurrentWeekRideCount { get; set; }
    public bool CurrentWeekGoalMet { get; set; }
    public List<int> CurrentWeekRideDays { get; set; } = [];
}