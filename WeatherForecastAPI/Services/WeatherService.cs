using WeatherForecastAPI.Interfaces;

namespace WeatherForecastAPI.Services
{
    public class WeatherService(HttpClient httpClient) : IWeatherService
    {
        private readonly HttpClient _httpClient = httpClient;
        private const string OpenMeteoApi = "https://api.open-meteo.com/v1/forecast?latitude={0}&longitude={1}&hourly=temperature_2m";
        public async Task<string> GetForecastAsync(double latitude, double longitude)
        {
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&hourly=temperature_2m";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
