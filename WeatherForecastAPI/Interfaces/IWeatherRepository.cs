using WeatherForecastAPI.Models;
namespace WeatherForecastAPI.Interfaces;

public interface IWeatherRepository
{
    Task<IEnumerable<Location>> GetAllAsync();
    Task<Location?> GetByIdAsync(int id);
    Task AddAsync(Location location);
    Task DeleteAsync(int id);
    Task SaveAsync();
}
