using AutoMapper;
using AuthenticationService.Application.DTOs;
using AuthenticationService.Domain.Entities;

namespace AuthenticationService.Application.Mappings
{
    /// <summary>
    /// AutoMapper profile for Authentication Service
    /// Maps between Domain Entities and DTOs
    /// </summary>
    public class AuthenticationMappingProfile : Profile
    {
        public AuthenticationMappingProfile()
        {
            // Map User entity to UserDto
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            // Map UserDto to User entity
            CreateMap<UserDto, User>();
        }
    }
}
