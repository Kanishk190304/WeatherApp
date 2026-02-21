using WeatherService.Application.DTOs;

namespace WeatherService.Application.Interfaces
{
    // Contract for weather service operations
    public interface IWeatherService
    {
        Task<WeatherResponseDto> GetWeatherAsync(WeatherRequestDto request);
    }
}