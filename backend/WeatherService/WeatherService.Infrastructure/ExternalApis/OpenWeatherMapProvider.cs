using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using WeatherService.Domain.Entities;
using WeatherService.Domain.Interfaces;

namespace WeatherService.Infrastructure.ExternalApis
{
    // Implementation of IWeatherProvider using OpenWeatherMap API
    public class OpenWeatherMapProvider : IWeatherProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public OpenWeatherMapProvider(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenWeatherMap:ApiKey"]!;
            _baseUrl = configuration["OpenWeatherMap:BaseUrl"]!;
        }

        // Fetch weather data from OpenWeatherMap API
        public async Task<WeatherData?> GetWeatherAsync(string city)
        {
            try
            {
                // Build API URL with city name and API key
                var url = $"{_baseUrl}?q={city}&appid={_apiKey}&units=metric";

                // Make HTTP request
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                // Parse JSON response
                var json = await response.Content.ReadAsStringAsync();
                var data = JObject.Parse(json);

                // Map JSON to WeatherData entity
                return new WeatherData
                {
                    City = data["name"]?.ToString() ?? city,
                    Temperature = data["main"]?["temp"]?.Value<double>() ?? 0,
                    Humidity = data["main"]?["humidity"]?.Value<int>() ?? 0,
                    Description = data["weather"]?[0]?["description"]?.ToString() ?? string.Empty,
                    Icon = data["weather"]?[0]?["icon"]?.ToString() ?? string.Empty,
                    Latitude = data["coord"]?["lat"]?.Value<double>() ?? 0,
                    Longitude = data["coord"]?["lon"]?.Value<double>() ?? 0,
                    Timestamp = DateTime.UtcNow
                };
            }
            catch
            {
                return null;
            }
        }
    }
}