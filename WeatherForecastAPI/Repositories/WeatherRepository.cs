using Microsoft.EntityFrameworkCore;
using WeatherForecastAPI.Data;
using WeatherForecastAPI.Interfaces;
using WeatherForecastAPI.Models;
namespace WeatherForecastAPI.Repositories;

public class WeatherRepository(WeatherDbContext context) : IWeatherRepository
{
    private readonly WeatherDbContext _context = context;

    public async Task<IEnumerable<Location>> GetAllAsync() => await _context.Locations.ToListAsync();

    public async Task<Location?> GetByIdAsync(int id) => await _context.Locations.FindAsync(id);

    public async Task AddAsync(Location location)
    {
        _context.Locations.Add(location);
        await SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var location = await GetByIdAsync(id);
        if (location != null)
        {
            _context.Locations.Remove(location);
            await SaveAsync();
        }
    }

    public async Task SaveAsync() => await _context.SaveChangesAsync();
}
