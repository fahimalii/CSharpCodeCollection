using FunctionalCSharp.Api.Domain.Primitives;
using FunctionalCSharp.Api.Domain.Primitives.Result;
using FunctionalCSharp.Api.ResponseModels;

namespace FunctionalCSharp.Api.Services;

public interface ISampleService
{
    Result<WeatherForecast> GetResults(string value);
}

public sealed class SampleService : ISampleService
{
    public Result<WeatherForecast> GetResults(string value)
    {
        if (value == null || value.Length != 5)
        {
            return Result.Failure<WeatherForecast>(new Error("Value.InvalidLength", "Invalid Value"));
        }

        var data = new WeatherForecast()
        {
            Date = DateOnly.FromDateTime(DateTime.UtcNow),
            Summary = "Test summary",
            TemperatureC = 32,
        };

        return Result.Success(data);
    }
}
