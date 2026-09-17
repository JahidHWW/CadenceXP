using CadenceXP.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CadenceXP.Api.Data;

public class CadenceXpDbContext : DbContext
{
    public CadenceXpDbContext(DbContextOptions<CadenceXpDbContext> options)
        : base(options)
    {
    }

    public DbSet<Ride> Rides { get; set; }
}