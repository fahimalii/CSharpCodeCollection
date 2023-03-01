using ExplorePolly.Api.Clients;
using Microsoft.AspNetCore.Mvc;

namespace ExplorePolly.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherClient _weatherClient;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherClient weatherClient)
    {
        _logger = logger;
        _weatherClient = weatherClient;
    }

    [HttpGet("weather/{city}")]
    public async Task<IActionResult> Forecast(string city)
    {
        var weather = await _weatherClient.GetCurrentWeatherForCity(city);

        return weather is not null ? Ok(weather) : NotFound();
    }

}
