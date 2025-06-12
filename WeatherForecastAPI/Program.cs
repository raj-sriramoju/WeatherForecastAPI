using Microsoft.EntityFrameworkCore;
using WeatherForecastAPI;
using WeatherForecastAPI.Data;
using WeatherForecastAPI.Interfaces;
using WeatherForecastAPI.Repositories;
using WeatherForecastAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WeatherDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddHttpClient<IWeatherService, WeatherService>();
builder.Services.AddAutoMapper(typeof(LocationMappingProfile));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
