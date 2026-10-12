using CadenceXP.Api.Data;
using CadenceXP.Api.Dtos;
using CadenceXP.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CadenceXP.Api.Services.Xp;

namespace CadenceXP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly CadenceXpDbContext _database;
    private readonly XpService _xpService;

    public UsersController(CadenceXpDbContext database, XpService xpService)
    {
        _database = database;
        _xpService = xpService;
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

        int totalXp = await _database.Rides
        .Where(ride => ride.UserId == id)
        .SumAsync(ride => ride.XpEarned);

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
}