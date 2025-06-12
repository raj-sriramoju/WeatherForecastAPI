using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WeatherForecastAPI.DTOs;
using WeatherForecastAPI.Interfaces;
using WeatherForecastAPI.Models;
namespace WeatherForecastAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WeatherController(IWeatherRepository repository, IWeatherService weatherService, IMapper mapper) : ControllerBase
{
    private readonly IWeatherRepository _repository = repository;
    private readonly IWeatherService _weatherService = weatherService;
    
    // GET: api/weather
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var locations = await _repository.GetAllAsync();
        var dtos = mapper.Map<IEnumerable<LocationDto>>(locations);
        return Ok(dtos);
    }

    // GET: api/weather/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var location = await _repository.GetByIdAsync(id);
        if (location == null) return NotFound();

        var dto = mapper.Map<LocationDto>(location);
        return Ok(dto);
    }

    // POST: api/weather
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] LocationDto locationDto)
    {

        var location = mapper.Map<Location>(locationDto);

        var forecast = await _weatherService.GetForecastAsync(location.Latitude, location.Longitude);
        location.ForecastSummary = forecast;

        await _repository.AddAsync(location);
        var createdDto = mapper.Map<LocationDto>(location);
        return CreatedAtAction(nameof(Get), new { id = location.Id }, location);
    }

    // DELETE: api/weather/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return NotFound();

        await _repository.DeleteAsync(id);
        return NoContent();
    }


    // GET: api/weather/{id}/latest
    [HttpGet("{id}/latest")]
    public async Task<IActionResult> GetLatestForecast(int id)
    {
        var location = await _repository.GetByIdAsync(id);
        if (location == null) return NotFound();

        var forecast = await _weatherService.GetForecastAsync(location.Latitude, location.Longitude);
        return Ok(forecast);
    }
}
