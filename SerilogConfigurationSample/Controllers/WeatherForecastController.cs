using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SerilogConfigurationSample.Services;

namespace SerilogConfigurationSample.Controllers;
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

    [HttpGet]
    public IActionResult GetData([FromServices] CorelationIdProvider corelationIdProvider, [FromServices] ISampleService sampleService)
    {
        var weatherForecast = new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        };

        _logger.LogInformation("Response {@WeatherForecast}. Corelation: {TestCorelationId}", weatherForecast, corelationIdProvider.CorelationId);
        _logger.LogInformation("Test {$WeatherForecast}", weatherForecast);

        sampleService.Test();

        return Ok(weatherForecast);
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var data = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();

        _logger.LogInformation(
            "Responding {@WeatherForecastList}", data);


        var person = new { FirstName = "Jon", LastName = "Doe" };
        var count = 0;

        _logger.LogInformation("Request for person {@Person} at time {Count}", person, count);

        return data;
    }
}
