using CadenceXP.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CadenceXP.Api.Data;

public class CadenceXpDbContext : DbContext
{
    public CadenceXpDbContext(DbContextOptions<CadenceXpDbContext> options)
        : base(options)
    {
    }

    public DbSet<AppUser> Users {get; set;} = null!;
    public DbSet<Bike> Bikes {get; set;} = null!;
    public DbSet<Ride> Rides { get; set;} = null!;

}