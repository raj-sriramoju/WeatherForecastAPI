using AutoMapper;
using WeatherForecastAPI.DTOs;
using WeatherForecastAPI.Models;
namespace WeatherForecastAPI;

public class LocationMappingProfile : Profile
{
    public LocationMappingProfile()
    {
        CreateMap<LocationDto, Location>();
        CreateMap<Location, LocationDto>();
    }
}
