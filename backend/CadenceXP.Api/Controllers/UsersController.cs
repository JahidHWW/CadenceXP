using CadenceXP.Api.Data;
using CadenceXP.Api.Dtos;
using CadenceXP.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CadenceXP.Api.Services.Xp;
using CadenceXP.Api.Models.Enums;
using CadenceXP.Api.Services.Quests;

namespace CadenceXP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly CadenceXpDbContext _database;
    private readonly XpService _xpService;
    private readonly QuestUnlockService _questUnlockService;

    public UsersController(
        CadenceXpDbContext database,
        XpService xpService,
        QuestUnlockService questUnlockService)
    {
        _database = database;
        _xpService = xpService;
        _questUnlockService = questUnlockService;
    }

    [HttpGet]
    public async Task<ActionResult<List<AppUser>>> GetUsers()
    {
        var users = await _database.Users.ToListAsync();

        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUserById(int id)
    {
        var user = await _database.Users
            .FirstOrDefaultAsync(user => user.Id == id);

        if (user is null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<AppUser>> CreateUser(
        CreateUserRequest request)
    {
        var user = new AppUser
        {
            DisplayName = request.DisplayName,
            Email = request.Email
        };

        _database.Users.Add(user);

        await _database.SaveChangesAsync();
        await _questUnlockService.UnlockEligibleQuestsAsync(user.Id);

        return Created($"/api/users/{user.Id}", user);
    }

    [HttpGet("{id}/bikes")]
    public async Task<ActionResult<List<Bike>>> GetBikesByUserId(int id)
    {
        var bikes = await _database.Bikes
        .Where(bike => bike.UserId == id)
        .ToListAsync();

        return Ok(bikes);
    }

    [HttpGet("{id}/progress")]
    public async Task<ActionResult<UserProgressResponse>> GetUserProgress(
        int id
    )
    {
        var userExists = await _database.Users
        .AnyAsync(user => user.Id == id);

        if (!userExists)
        {
            return NotFound();
        }

        int rideXp = await _database.Rides
            .Where(ride =>
                ride.UserId == id &&
                ride.ProcessingStatus == RideProcessingStatus.Completed)
            .SumAsync(ride => ride.XpEarned);

        int questXp = await _database.UserQuests
            .Where(userQuest => userQuest.UserId == id &&
                userQuest.IsCompleted)
            .SumAsync(userQuest => userQuest.Quest.XpReward);

        int totalXp = rideXp + questXp;

        var level = _xpService.CalculateLevel(totalXp);
        var xpIntoLevel =
            _xpService.CalculateXpIntoLevel(totalXp);
        var xpForNextLevel =
            _xpService.CalculateXpForNextLevel(level);

        var response = new UserProgressResponse
        {
            UserId = id,
            TotalXp = totalXp,
            Level = level,
            XpIntoLevel = xpIntoLevel,
            XpForNextLevel = xpForNextLevel
        };

        return Ok(response);
    }

    [HttpGet("{id}/quests")]
    public async Task<ActionResult<List<UserQuestResponse>>> GetUserQuests(int id)
    {
        var userExists = await _database.Users
            .AnyAsync(user => user.Id == id);

        if (!userExists)
        {
            return NotFound();
        }

        var quests = await (
            from quest in _database.Quests

            join userQuest in _database.UserQuests
                .Where(userQuest => userQuest.UserId == id)

            on quest.Id equals userQuest.QuestId
            into userQuestGroup
            from userQuest in userQuestGroup.DefaultIfEmpty()

            select new UserQuestResponse
            {
                QuestId = quest.Id,
                Name = quest.Name,
                Description = quest.Description,
                Type = quest.Type,
                TargetValue = quest.TargetValue,
                XpReward = quest.XpReward,

                Progress = userQuest != null
                ? userQuest.Progress
                : 0,

                IsCompleted = userQuest != null
                ? userQuest.IsCompleted
                : false,

                CompletedAtUtc = userQuest != null
                ? userQuest.CompletedAtUtc
                : null
            }
        )
        .ToListAsync();
        return Ok(quests);
    }
}