using Microsoft.AspNetCore.Mvc;
using SwaggerCustomization.Api.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace SwaggerCustomization.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet]
    public IActionResult Test([FromHeader] Sample request)
    {
        return Ok(request);
    }


}

public class Sample
{
    [FromHeader]
    [SwaggerIgnore]
    public string Name { get; set; }

    [SwaggerIgnore]
    public string Test123 { get; set; }
}
