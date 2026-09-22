using CadenceXP.Api.Data;
using CadenceXP.Api.Dtos;
using CadenceXP.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CadenceXP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly CadenceXpDbContext _database;

    public UsersController(CadenceXpDbContext database)
    {
        _database = database;
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

        if(user is null)
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
}