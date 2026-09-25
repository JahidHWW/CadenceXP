using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CadenceXP.Api.Dtos;

public class UploadRideRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public required string Name { get; set; }

    [Range(1, int.MaxValue)]
    public int UserId { get; set; }

    [Range(1, int.MaxValue)]
    public int BikeId { get; set; }

    [Required]
    public required IFormFile File { get; set; }
}