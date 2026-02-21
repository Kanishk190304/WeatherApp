using AuthenticationService.Application.DTOs;
using FluentValidation;

namespace AuthenticationService.Application.Validators
{
    // Validates login request data
    public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestValidator()
        {
            // Email validation rules
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            // Password validation rules
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}