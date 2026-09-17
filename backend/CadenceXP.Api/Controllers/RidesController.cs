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
    public async Task<ActionResult<List<Ride>>> GetRides()
    {
        var rides = await _database.Rides.ToListAsync();

        return Ok(rides);
    }

    [HttpPost]
    public async Task<ActionResult<Ride>> CreateRide(
        CreateRideRequest request)
    {
        var ride = new Ride
        {
            Name = request.Name,
            DistanceMiles = request.DistanceMiles,
            RideDate = request.RideDate
        };

        _database.Rides.Add(ride);

        await _database.SaveChangesAsync();

        return Created($"/api/rides/{ride.Id}", ride);
    }
}