using CadenceXP.Api.Data;
using CadenceXP.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CadenceXP.Api.Dtos;
using CadenceXP.Api.Models.Enums;
using CadenceXP.Api.Services.Gpx;

namespace CadenceXP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RidesController : ControllerBase
{
    private readonly CadenceXpDbContext _database;
    private readonly IWebHostEnvironment _environment;
    private readonly GpxParser _gpxParser;

    public RidesController(
        CadenceXpDbContext database,
        IWebHostEnvironment environment,
        GpxParser gpxParser)
    {
        _database = database;
        _environment = environment;
        _gpxParser = gpxParser;
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
                RideDateUtc = ride.RideDateUtc,
                OriginalFileName = ride.OriginalFileName,
                ProcessingStatus = ride.ProcessingStatus
            })
            .ToListAsync();

        return Ok(rides);
    }

    [HttpPost("upload")]
    public async Task<ActionResult<Ride>> UploadRide(
        [FromForm] UploadRideRequest request)
    {
        var userExists = await _database.Users
            .AnyAsync(user => user.Id == request.UserId);

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

        var extension = Path.GetExtension(request.File.FileName);

        if (!extension.Equals(".gpx", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Only GPX files are supported.");
        }

        if (request.File.Length == 0)
        {
            return BadRequest("The uploaded GPX file is empty.");
        }

        var ride = new Ride
        {
            Name = request.Name,
            UserId = request.UserId,
            BikeId = request.BikeId,

            DistanceMeters = 0,
            ElevationGainMeters = 0,
            DurationSeconds = 0,

            RideDateUtc = DateTime.UtcNow,

            OriginalFileName = Path.GetFileName(request.File.FileName),

            ProcessingStatus = RideProcessingStatus.Pending
        };

        _database.Rides.Add(ride);
        await _database.SaveChangesAsync();

        var uploadDirectory = Path.Combine(
            _environment.ContentRootPath,
            "uploads",
            "gpx"
        );

        Directory.CreateDirectory(uploadDirectory);

        var filePath = Path.Combine(
            uploadDirectory,
            $"{ride.Id}.gpx"
        );

        await using var fileStream = new FileStream(
            filePath,
            FileMode.Create
        );

        await request.File.CopyToAsync(fileStream);

        return Created($"/api/rides/{ride.Id}", ride);
    }

    [HttpGet("{id}/track-points")]
    public async Task<ActionResult<List<GpxTrackPoint>>> GetTrackPoints(int id)
    {
        var ride = await _database.Rides.FindAsync(id);

        if (ride is null)
        {
            return NotFound("Ride does not exist.");
        }

        var filePath = Path.Combine(
            _environment.ContentRootPath,
            "uploads",
            "gpx",
            $"{ride.Id}.gpx"
        );

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("GPX file does not exist for this ride.");
        }

        var trackPoints = _gpxParser.Parse(filePath);

        return Ok(trackPoints);
    }

    [HttpPost("{id}/process")]
    public async Task<ActionResult<RideResponse>> ProcessRide(int id)
    {
        var ride = await _database.Rides
            .Include(ride => ride.User)
            .Include(ride => ride.Bike)
            .FirstOrDefaultAsync(ride => ride.Id == id);

        if (ride is null)
        {
            return NotFound("Ride does not exist.");
        }

        var filePath = Path.Combine(
            _environment.ContentRootPath,
            "uploads",
            "gpx",
            $"{ride.Id}.gpx"
        );

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("GPX file does not exist for this ride.");
        }

        try
        {
            ride.ProcessingStatus = RideProcessingStatus.Processing;
            await _database.SaveChangesAsync();

            var trackPoints = _gpxParser.Parse(filePath);

            if (trackPoints.Count < 2)
            {
                ride.ProcessingStatus = RideProcessingStatus.Failed;
                await _database.SaveChangesAsync();

                return BadRequest(
                    "The GPX file does not contain enough track points."
                );
            }

            ride.DistanceMeters =
                _gpxParser.CalculateDistanceMeters(trackPoints);

            ride.ElevationGainMeters =
                _gpxParser.CalculateElevationGainMeters(trackPoints);

            ride.DurationSeconds =
                _gpxParser.CalculateDurationSeconds(trackPoints);

            ride.RideDateUtc = trackPoints[0].TimeUtc;

            ride.ProcessingStatus = RideProcessingStatus.Completed;

            await _database.SaveChangesAsync();

            var response = new RideResponse
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
            };

            return Ok(response);
        }
        catch
        {
            ride.ProcessingStatus = RideProcessingStatus.Failed;
            await _database.SaveChangesAsync();

            return StatusCode(
                500,
                "The GPX file could not be processed."
            );
        }
    }
}