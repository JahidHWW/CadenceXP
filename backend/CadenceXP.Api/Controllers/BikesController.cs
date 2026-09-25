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

    [HttpGet("{id}/rides")]
    public async Task<ActionResult<List<RideResponse>>> GetBikeRides(int id)
    {
        var bikeExists = await _database.Bikes
            .AnyAsync(bike => bike.Id == id);

        if (!bikeExists)
        {
            return NotFound();
        }

        var rides = await _database.Rides
            .Where(ride => ride.BikeId == id)
            .Select(ride => new RideResponse
            {
                Id = ride.Id,
                Name = ride.Name,
                UserId = ride.UserId,
                UserDisplayName = ride.User.DisplayName,
                BikeId = ride.BikeId,
                BikeName = ride.Bike.Name,
                DistanceMeters = ride.DistanceMeters,
                ElevationGainMeters = ride.ElevationGainMeters,
                DurationSeconds = ride.DurationSeconds,
                RideDateUtc = ride.RideDateUtc,
                OriginalFileName = ride.OriginalFileName,
                ProcessingStatus = ride.ProcessingStatus
            })
            .ToListAsync();

        return Ok(rides);
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