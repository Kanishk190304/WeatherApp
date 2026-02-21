using WeatherService.Application.Builders;
using WeatherService.Application.DTOs;
using WeatherService.Application.Interfaces;
using WeatherService.Domain.Entities;
using WeatherService.Domain.Interfaces;

namespace WeatherService.Infrastructure.Services
{
    // Implementation of IWeatherService with caching
    public class WeatherDataService : IWeatherService
    {
        private readonly IWeatherProvider _weatherProvider;
        private readonly ICacheService _cacheService;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(10);

        public WeatherDataService(IWeatherProvider weatherProvider, ICacheService cacheService)
        {
            _weatherProvider = weatherProvider;
            _cacheService = cacheService;
        }

        // Get weather data with caching
        public async Task<WeatherResponseDto> GetWeatherAsync(WeatherRequestDto request)
        {
            // Generate cache key from city name
            var cacheKey = $"weather:{request.City.ToLower().Trim()}";

            // Try to get from cache first
            var cachedData = await _cacheService.GetAsync<WeatherData>(cacheKey);
            
            if (cachedData != null)
            {
                // Return cached data
                return new WeatherResponseBuilder()
                    .WithSuccess(true)
                    .WithMessage("Weather data retrieved from cache")
                    .FromWeatherData(cachedData)
                    .WithCacheStatus(true)
                    .Build();
            }

            // Fetch from external API
            var weatherData = await _weatherProvider.GetWeatherAsync(request.City);

            if (weatherData == null)
            {
                return WeatherResponseBuilder.BuildError($"Could not find weather data for city: {request.City}");
            }

            // Store in cache for 10 minutes
            await _cacheService.SetAsync(cacheKey, weatherData, _cacheExpiration);

            // Return fresh data
            return new WeatherResponseBuilder()
                .WithSuccess(true)
                .WithMessage("Weather data retrieved from API")
                .FromWeatherData(weatherData)
                .WithCacheStatus(false)
                .Build();
        }
    }
}