using ExplorePolly.Api.Constants;
using ExplorePolly.Api.Models;

namespace ExplorePolly.Api.Clients;

public interface IWeatherClient
{
    Task<WeatherResponse?> GetCurrentWeatherForCity(string city);
}
public class OpenWeatherClient : IWeatherClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public OpenWeatherClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<WeatherResponse?> GetCurrentWeatherForCity(string city)
    {
        try
        {
            var client = _httpClientFactory.CreateClient(AppConstants.OpenWeatherHttpClientName);

            return await client.GetFromJsonAsync<WeatherResponse?>(
                $"weather?q={city}&appid={AppConstants.OpenWeatherApiKey}");
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}



