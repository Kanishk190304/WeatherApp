namespace WeatherService.Application.DTOs
{
    // Data transfer object for weather response
    public class WeatherResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool FromCache { get; set; }
    }
}