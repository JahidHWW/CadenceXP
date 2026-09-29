using CadenceXP.Api.Data;
using CadenceXP.Api.Dtos;
using CadenceXP.Api.Models;
using CadenceXP.Api.Models.Enums;
using CadenceXP.Api.Services.Gpx;
using CadenceXP.Api.Services.Rides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CadenceXP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RidesController : ControllerBase
{
    private readonly CadenceXpDbContext _database;
    private readonly IWebHostEnvironment _environment;
    private readonly GpxParser _gpxParser;
    private readonly RideProcessingService _rideProcessingService;

    public RidesController(
        CadenceXpDbContext database,
        IWebHostEnvironment environment,
        GpxParser gpxParser,
        RideProcessingService rideProcessingService)
    {
        _database = database;
        _environment = environment;
        _gpxParser = gpxParser;
        _rideProcessingService = rideProcessingService;
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

    [HttpGet("{id}")]
    public async Task<ActionResult<RideResponse>> GetRideById(int id)
    {
        var ride = await _database.Rides
            .Where(ride => ride.Id == id)
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
            .FirstOrDefaultAsync();

        if (ride is null)
        {
            return NotFound("Ride does not exist.");
        }

        return Ok(ride);
    }

    [HttpPost("upload")]
    public async Task<ActionResult<RideResponse>> UploadRide(
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

        if (!extension.Equals(
            ".gpx",
            StringComparison.OrdinalIgnoreCase))
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

            OriginalFileName =
                Path.GetFileName(request.File.FileName),

            ProcessingStatus =
                RideProcessingStatus.Pending
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

        // Save the uploaded GPX.
        // The stream is closed when this block ends.
        {
            await using var fileStream = new FileStream(
                filePath,
                FileMode.Create
            );

            await request.File.CopyToAsync(fileStream);
        }

        try
        {
            ride.ProcessingStatus =
                RideProcessingStatus.Processing;

            await _database.SaveChangesAsync();

            var result =
                _rideProcessingService.Process(filePath);

            ride.DistanceMeters =
                result.DistanceMeters;

            ride.ElevationGainMeters =
                result.ElevationGainMeters;

            ride.DurationSeconds =
                result.DurationSeconds;

            ride.RideDateUtc =
                result.RideDateUtc;

            ride.ProcessingStatus =
                RideProcessingStatus.Completed;

            await _database.SaveChangesAsync();
        }
        catch (InvalidOperationException)
        {
            ride.ProcessingStatus =
                RideProcessingStatus.Failed;

            await _database.SaveChangesAsync();
        }
        catch
        {
            ride.ProcessingStatus =
                RideProcessingStatus.Failed;

            await _database.SaveChangesAsync();
        }

        var response = new RideResponse
        {
            Id = ride.Id,
            Name = ride.Name,

            UserId = ride.UserId,
            UserDisplayName = await _database.Users
                .Where(user => user.Id == ride.UserId)
                .Select(user => user.DisplayName)
                .FirstAsync(),

            BikeId = ride.BikeId,
            BikeName = await _database.Bikes
                .Where(bike => bike.Id == ride.BikeId)
                .Select(bike => bike.Name)
                .FirstAsync(),

            DistanceMeters = ride.DistanceMeters,
            ElevationGainMeters = ride.ElevationGainMeters,
            DurationSeconds = ride.DurationSeconds,
            RideDateUtc = ride.RideDateUtc,
            OriginalFileName = ride.OriginalFileName,
            ProcessingStatus = ride.ProcessingStatus
        };

        return Created(
            $"/api/rides/{ride.Id}",
            response
        );
    }

    [HttpGet("{id}/track-points")]
    public async Task<ActionResult<List<GpxTrackPoint>>> GetTrackPoints(
        int id)
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
            return NotFound(
                "GPX file does not exist for this ride."
            );
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
            return NotFound(
                "GPX file does not exist for this ride."
            );
        }

        try
        {
            ride.ProcessingStatus =
                RideProcessingStatus.Processing;

            await _database.SaveChangesAsync();

            var result =
                _rideProcessingService.Process(filePath);

            ride.DistanceMeters =
                result.DistanceMeters;

            ride.ElevationGainMeters =
                result.ElevationGainMeters;

            ride.DurationSeconds =
                result.DurationSeconds;

            ride.RideDateUtc =
                result.RideDateUtc;

            ride.ProcessingStatus =
                RideProcessingStatus.Completed;

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
                ElevationGainMeters =
                    ride.ElevationGainMeters,

                DurationSeconds = ride.DurationSeconds,
                RideDateUtc = ride.RideDateUtc,

                OriginalFileName = ride.OriginalFileName,
                ProcessingStatus = ride.ProcessingStatus
            };

            return Ok(response);
        }
        catch (InvalidOperationException exception)
        {
            ride.ProcessingStatus =
                RideProcessingStatus.Failed;

            await _database.SaveChangesAsync();

            return BadRequest(exception.Message);
        }
        catch
        {
            ride.ProcessingStatus =
                RideProcessingStatus.Failed;

            await _database.SaveChangesAsync();

            return StatusCode(
                500,
                "The GPX file could not be processed."
            );
        }
    }
}