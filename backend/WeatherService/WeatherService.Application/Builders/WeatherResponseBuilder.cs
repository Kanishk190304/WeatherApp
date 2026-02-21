using WeatherService.Application.DTOs;
using WeatherService.Domain.Entities;

namespace WeatherService.Application.Builders
{
    // Builder Pattern: Constructs WeatherResponseDto step by step
    public class WeatherResponseBuilder
    {
        private readonly WeatherResponseDto _response;

        public WeatherResponseBuilder()
        {
            _response = new WeatherResponseDto();
        }

        // Set success status
        public WeatherResponseBuilder WithSuccess(bool success)
        {
            _response.Success = success;
            return this;
        }

        // Set message
        public WeatherResponseBuilder WithMessage(string message)
        {
            _response.Message = message;
            return this;
        }

        // Set city name
        public WeatherResponseBuilder WithCity(string city)
        {
            _response.City = city;
            return this;
        }

        // Set temperature
        public WeatherResponseBuilder WithTemperature(double temperature)
        {
            _response.Temperature = temperature;
            return this;
        }

        // Set humidity
        public WeatherResponseBuilder WithHumidity(int humidity)
        {
            _response.Humidity = humidity;
            return this;
        }

        // Set description
        public WeatherResponseBuilder WithDescription(string description)
        {
            _response.Description = description;
            return this;
        }

        // Set icon
        public WeatherResponseBuilder WithIcon(string icon)
        {
            _response.Icon = icon;
            return this;
        }

        // Set coordinates
        public WeatherResponseBuilder WithCoordinates(double latitude, double longitude)
        {
            _response.Latitude = latitude;
            _response.Longitude = longitude;
            return this;
        }

        // Set cache status
        public WeatherResponseBuilder WithCacheStatus(bool fromCache)
        {
            _response.FromCache = fromCache;
            return this;
        }

        // Build from WeatherData entity
        public WeatherResponseBuilder FromWeatherData(WeatherData data)
        {
            _response.City = data.City;
            _response.Temperature = data.Temperature;
            _response.Humidity = data.Humidity;
            _response.Description = data.Description;
            _response.Icon = data.Icon;
            _response.Latitude = data.Latitude;
            _response.Longitude = data.Longitude;
            return this;
        }

        // Build and return the final response
        public WeatherResponseDto Build()
        {
            return _response;
        }

        // Static method for error response
        public static WeatherResponseDto BuildError(string message)
        {
            return new WeatherResponseBuilder()
                .WithSuccess(false)
                .WithMessage(message)
                .Build();
        }
    }
}