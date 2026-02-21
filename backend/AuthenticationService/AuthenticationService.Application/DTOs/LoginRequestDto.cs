namespace AuthenticationService.Application.DTOs
{
    // Data transfer object for user login request
    public class LoginRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}