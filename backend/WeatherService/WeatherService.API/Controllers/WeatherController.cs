using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherService.Application.DTOs;
using WeatherService.Application.Interfaces;
using WeatherService.Application.Validators;

namespace WeatherService.API.Controllers
{
    // API controller for weather endpoints
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        // GET: api/weather?city=London
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetWeather([FromQuery] string city)
        {
            // Create request DTO
            var request = new WeatherRequestDto { City = city };

            // Validate request
            var validator = new WeatherRequestValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(new WeatherResponseDto
                {
                    Success = false,
                    Message = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
                });
            }

            // Get weather data
            var result = await _weatherService.GetWeatherAsync(request);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        // GET: api/weather/test - Unprotected endpoint for testing
        [HttpGet("test")]
        [AllowAnonymous]
        public IActionResult Test()
        {
            return Ok(new { Message = "Weather Service is running", Timestamp = DateTime.UtcNow });
        }
    }
}