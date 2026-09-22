using CadenceXP.Api.Data;
using CadenceXP.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CadenceXP.Api.Dtos;

namespace CadenceXP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RidesController : ControllerBase
{
    private readonly CadenceXpDbContext _database;

    public RidesController(CadenceXpDbContext database)
    {
        _database = database;
    }

    [HttpGet]
    public async Task<ActionResult<List<RideResponse>>> GetRides()
    {
        var rides = await _database.Rides
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
                RideDateUtc = ride.RideDateUtc
            })
            .ToListAsync();

        return Ok(rides);
    }

    [HttpPost]
    public async Task<ActionResult<Ride>> CreateRide(CreateRideRequest request)
    {
        var userExists = await _database.Users.AnyAsync(user => user.Id == request.UserId);

        if (!userExists)
        {
            return BadRequest("User does not exist.");
        }

        var bikeBelongsToUser = await _database.Bikes
            .AnyAsync(bike =>
                bike.Id == request.BikeId &&
                bike.UserId == request.UserId
            );

        if (!bikeBelongsToUser)
        {
            return BadRequest("Bike does not belong to this user.");
        }

        var ride = new Ride
        {
            Name = request.Name,
            UserId = request.UserId,
            BikeId = request.BikeId,
            DistanceMeters = request.DistanceMeters,
            ElevationGainMeters = request.ElevationGainMeters,
            DurationSeconds = request.DurationSeconds,
            RideDateUtc = request.RideDateUtc
        };

        _database.Rides.Add(ride);

        await _database.SaveChangesAsync();

        return Created($"/api/rides/{ride.Id}", ride);
    }
}