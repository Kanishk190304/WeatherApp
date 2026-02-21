using WeatherService.Domain.Entities;

namespace WeatherService.Domain.Interfaces
{
    // Contract for external weather API providers (Factory Pattern will use this)
    public interface IWeatherProvider
    {
        Task<WeatherData?> GetWeatherAsync(string city);
    }
}