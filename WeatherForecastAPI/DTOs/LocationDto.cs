namespace WeatherForecastAPI.DTOs;

public class LocationDto
{
    public int Id { get; set; }
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
    public string? ForecastSummary { get; set; }
}
