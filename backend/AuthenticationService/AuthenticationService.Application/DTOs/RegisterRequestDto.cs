namespace AuthenticationService.Application.DTOs
{
    // Data transfer object for user registration request
    public class RegisterRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}