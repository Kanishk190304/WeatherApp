using Microsoft.Extensions.Logging;
using WeatherService.Application.Builders;
using WeatherService.Application.DTOs;
using WeatherService.Application.Interfaces;
using WeatherService.Domain.Entities;
using WeatherService.Domain.Interfaces;

namespace WeatherService.Infrastructure.Services
{
    // Implementation of IWeatherService with caching and logging
    public class WeatherDataService : IWeatherService
    {
        private readonly IWeatherProvider _weatherProvider;
        private readonly ICacheService _cacheService;
        private readonly ILogger<WeatherDataService> _logger;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(10);

        public WeatherDataService(
            IWeatherProvider weatherProvider,
            ICacheService cacheService,
            ILogger<WeatherDataService> logger)
        {
            _weatherProvider = weatherProvider;
            _cacheService = cacheService;
            _logger = logger;
        }

        // Get weather data with caching
        public async Task<WeatherResponseDto> GetWeatherAsync(WeatherRequestDto request)
        {
            var city = request.City.ToLower().Trim();
            var cacheKey = $"weather:{city}";

            _logger.LogInformation("Weather request for city: {City}", city);

            try
            {
                // Try to get from cache first
                var cachedData = await _cacheService.GetAsync<WeatherData>(cacheKey);

                if (cachedData != null)
                {
                    _logger.LogInformation("Cache HIT for city: {City}", city);

                    return new WeatherResponseBuilder()
                        .WithSuccess(true)
                        .WithMessage("Weather data retrieved from cache")
                        .FromWeatherData(cachedData)
                        .WithCacheStatus(true)
                        .Build();
                }

                _logger.LogInformation("Cache MISS for city: {City}, fetching from API", city);

                // Fetch from external API
                var weatherData = await _weatherProvider.GetWeatherAsync(request.City);

                if (weatherData == null)
                {
                    _logger.LogWarning("Weather data not found for city: {City}", city);
                    return WeatherResponseBuilder.BuildError($"Could not find weather data for city: {request.City}");
                }

                // Store in cache
                await _cacheService.SetAsync(cacheKey, weatherData, _cacheExpiration);
                _logger.LogInformation("Weather data cached for city: {City}, TTL: {TTL} minutes", city, _cacheExpiration.TotalMinutes);

                return new WeatherResponseBuilder()
                    .WithSuccess(true)
                    .WithMessage("Weather data retrieved from API")
                    .FromWeatherData(weatherData)
                    .WithCacheStatus(false)
                    .Build();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching weather for city: {City}", city);
                return WeatherResponseBuilder.BuildError($"Error fetching weather data: {ex.Message}");
            }
        }
    }
}