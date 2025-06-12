using Microsoft.EntityFrameworkCore;
using WeatherForecastAPI.Models;
namespace WeatherForecastAPI.Data;

public class WeatherDbContext(DbContextOptions<WeatherDbContext> options) : DbContext(options)
{
    public DbSet<Location> Locations => Set<Location>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Location>().HasData(
            new Location { Id = 1, Latitude = 42.73, Longitude = -84.55 },
            new Location { Id = 2, Latitude = 42.33, Longitude = -83.04 }
        );
    }

}
