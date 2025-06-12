using Microsoft.EntityFrameworkCore;
using WeatherForecastAPI.Data;
using WeatherForecastAPI.Models;
using WeatherForecastAPI.Repositories;

namespace WeatherForecastAPI.Tests;

public class WeatherRepositoryTests
{
    private static WeatherDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<WeatherDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new WeatherDbContext(options);
    }

    private static async Task SeedDefaultLocationsAsync(WeatherDbContext context)
    {
        var defaultLocations = new[]
        {
            new Location { Id = 1, Latitude = 34.05, Longitude = -18.24 },
            new Location { Id = 2, Latitude = 51.50, Longitude = -10.12 }
         };

        context.Locations.AddRange(defaultLocations);
        await context.SaveChangesAsync();
    }


    [Fact]
    public async Task GetLocations_Should_Return_All_Locations()
    {
        using var context = CreateDbContext();
        var repository = new WeatherRepository(context);

        await SeedDefaultLocationsAsync(context);

        var locations = await repository.GetAllAsync();
        Assert.Equal(2, locations.Count());
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Requested_Location()
    {
        using var context = CreateDbContext();
        var repository = new WeatherRepository(context);

        await SeedDefaultLocationsAsync(context);

        var found = await repository.GetByIdAsync(1);
        Assert.NotNull(found);
        Assert.Equal(34.05, found?.Latitude);
    }

    [Fact]
    public async Task AddAsync_Should_Add_Location()
    {
        using var context = CreateDbContext();
        var repository = new WeatherRepository(context);

        await SeedDefaultLocationsAsync(context);

        var newLocation = new Location { Id = 3, Latitude = 45.15, Longitude = 52.34 };
        await repository.AddAsync(newLocation);

        var all = await repository.GetAllAsync();
        Assert.Equal(3, all.Count());
        Assert.Contains(all, loc => loc.Id == 3);
     
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Requested_Location()
    {
        using var context = CreateDbContext();
        var repository = new WeatherRepository(context);

        await SeedDefaultLocationsAsync(context);
        
        await repository.DeleteAsync(2);

        var all = await repository.GetAllAsync();
        Assert.Single(all);
        Assert.Equal(1, all.First().Id);
    }
}