using Microsoft.Extensions.Configuration;
using WeatherService.Domain.Interfaces;

namespace WeatherService.Infrastructure.ExternalApis
{
    // Factory Pattern: Creates weather provider instances based on configuration
    public interface IWeatherProviderFactory
    {
        IWeatherProvider CreateProvider(string providerName);
    }

    public class WeatherProviderFactory : IWeatherProviderFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public WeatherProviderFactory(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        // Create weather provider based on name (extensible for future providers)
        public IWeatherProvider CreateProvider(string providerName)
        {
            return providerName.ToLower() switch
            {
                "openweathermap" => new OpenWeatherMapProvider(
                    _httpClientFactory.CreateClient(), 
                    _configuration),
                
                // Add more providers here in future (e.g., "weatherapi", "accuweather")
                _ => throw new ArgumentException($"Unknown weather provider: {providerName}")
            };
        }
    }
}