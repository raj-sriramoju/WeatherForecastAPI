namespace WeatherForecastAPI.Interfaces;

public interface IWeatherService
{
    Task<string> GetForecastAsync(double latitude, double longitude);
}
