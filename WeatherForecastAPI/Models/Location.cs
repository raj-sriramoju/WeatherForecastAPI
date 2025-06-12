
using System.ComponentModel.DataAnnotations;
namespace WeatherForecastAPI.Models;

public class Location
{
    [Key]
    public int Id { get; set; }
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
    public string? ForecastSummary { get; set; }
    
}
