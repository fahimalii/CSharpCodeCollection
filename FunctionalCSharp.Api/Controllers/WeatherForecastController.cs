using FunctionalCSharp.Api.Domain.Primitives.Result;
using FunctionalCSharp.Api.ResponseModels;
using FunctionalCSharp.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FunctionalCSharp.Api.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ISampleService _sampleService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ISampleService sampleService)
    {
        _logger = logger;
        _sampleService = sampleService;
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

    [HttpPost]
    public IActionResult GetData(string value)
    {
        Result<WeatherForecast> opResult = _sampleService.GetResults(value);

        if(opResult.IsSuccess && opResult.Value is WeatherForecast weatherForecast) 
        {
            return Ok(weatherForecast);
        }
        else
        {
            return BadRequest(opResult.Error.Message);
        }
    }
}
