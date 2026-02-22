using AutoMapper;
using WeatherService.Application.DTOs;
using WeatherService.Domain.Entities;

namespace WeatherService.Application.Mappings
{
    /// <summary>
    /// AutoMapper profile for Weather Service
    /// Maps between Domain Entities and DTOs
    /// </summary>
    public class WeatherMappingProfile : Profile
    {
        public WeatherMappingProfile()
        {
            // Map WeatherData entity to WeatherResponseDto
            CreateMap<WeatherData, WeatherResponseDto>()
                .ForMember(dest => dest.Success, opt => opt.Ignore())
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .ForMember(dest => dest.FromCache, opt => opt.Ignore());

            // Map WeatherResponseDto to WeatherData entity
            CreateMap<WeatherResponseDto, WeatherData>();
        }
    }
}
