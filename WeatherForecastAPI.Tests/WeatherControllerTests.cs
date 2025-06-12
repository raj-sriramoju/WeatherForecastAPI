using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WeatherForecastAPI.Controllers;
using WeatherForecastAPI.DTOs;
using WeatherForecastAPI.Interfaces;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.Tests;

public class WeatherControllerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IWeatherRepository> _mockRepo;
    private readonly Mock<IWeatherService> _mockService;
    private readonly WeatherController _controller;

    public WeatherControllerTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new LocationMappingProfile());
        });
        _mapper = config.CreateMapper();

        _mockRepo = new Mock<IWeatherRepository>();
        _mockService = new Mock<IWeatherService>();
        _controller = new WeatherController(_mockRepo.Object, _mockService.Object, _mapper);
    }

    [Fact]
    public async Task GetAll_Should_Return_All_Locations_Dto()
    {
        var entities = new List<Location>
        {
            new() { Id = 1, Latitude = 10, Longitude = 20, ForecastSummary = "Sunny" }
        };

        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var dtos = Assert.IsAssignableFrom<IEnumerable<LocationDto>>(okResult.Value);
        Assert.Single(dtos);
    }

    [Fact]
    public async Task Get_Should_Return_Location_Dto_When_Exists()
    {
        var location = new Location { Id = 1, Latitude = 1, Longitude = 2, ForecastSummary = "Rainy" };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(location);

        var result = await _controller.Get(1);

        var ok = Assert.IsType<OkObjectResult>(result);
        var dto = Assert.IsType<LocationDto>(ok.Value);
        Assert.Equal("Rainy", dto.ForecastSummary);
    }

    [Fact]
    public async Task Get_Should_Returns_NotFound_WhenMissing()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Location?)null);
        var result = await _controller.Get(999);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Add_Should_Returns_Created_Location_WithDto()
    {
        var inputDto = new LocationDto { Latitude = 12.3, Longitude = 45.6 };
        _mockService.Setup(s => s.GetForecastAsync(inputDto.Latitude, inputDto.Longitude))
                    .ReturnsAsync("Cloudy");

        var result = await _controller.Add(inputDto);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        var returnedDto = Assert.IsType<Location>(created.Value);
        Assert.Equal("Cloudy", returnedDto.ForecastSummary);
    }

    [Fact]
    public async Task Delete_Should_Delete_And_Return_NoContent_If_Exists()
    {
        var location = new Location { Id = 1, Latitude = 12.12, Longitude = 10.12 };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(location);

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_Should_Return_NotFound_If_Missing()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(42)).ReturnsAsync((Location?)null);

        var result = await _controller.Delete(42);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetLatestForecast_Should_Return_Forecast_When_Exists()
    {
        var location = new Location { Id = 1, Latitude = 44.0, Longitude = -75.0 };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(location);
        _mockService.Setup(s => s.GetForecastAsync(location.Latitude, location.Longitude))
                    .ReturnsAsync("Sunny and Windy");

        var result = await _controller.GetLatestForecast(1);

        var ok = Assert.IsType<OkObjectResult>(result);
        var forecast = Assert.IsType<string>(ok.Value);
        Assert.Contains("Sunny", forecast);
    }

    [Fact]
    public async Task GetLatestForecast_Should_Return_NotFound_If_Missing()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Location?)null);

        var result = await _controller.GetLatestForecast(999);

        Assert.IsType<NotFoundResult>(result);
    }
}
