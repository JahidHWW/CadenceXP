using CadenceXP.Api.Data;
using CadenceXP.Api.Dtos;
using CadenceXP.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CadenceXP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BikesController : ControllerBase
{
    private readonly CadenceXpDbContext _database;

    public BikesController(CadenceXpDbContext database)
    {
        _database = database;
    }

    [HttpGet]
    public async Task<ActionResult<List<Bike>>> GetBikes()
    {
        var bikes = await _database.Bikes.ToListAsync();

        return Ok(bikes);
    }

    [HttpPost]
    public async Task<ActionResult<Bike>> CreateBike(
        CreateBikeRequest request)
    {
        var userExists = await _database.Users
            .AnyAsync(user => user.Id == request.UserId);

        if (!userExists)
        {
            return BadRequest("User does not exist.");
        }

        var bike = new Bike
        {
            Name = request.Name,
            Type = request.Type,
            UserId = request.UserId
        };

        _database.Bikes.Add(bike);

        await _database.SaveChangesAsync();

        return Created($"/api/bikes/{bike.Id}", bike);
    }
}