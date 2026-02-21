using FluentValidation;
using WeatherService.Application.DTOs;

namespace WeatherService.Application.Validators
{
    // Validates weather request data
    public class WeatherRequestValidator : AbstractValidator<WeatherRequestDto>
    {
        public WeatherRequestValidator()
        {
            // City validation rules
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City name is required")
                .MaximumLength(100).WithMessage("City name must not exceed 100 characters")
                .Matches(@"^[a-zA-Z\s\-]+$").WithMessage("City name can only contain letters, spaces, and hyphens");
        }
    }
}