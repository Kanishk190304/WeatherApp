namespace AuthenticationService.Application.DTOs
{
    // Data transfer object for Google authentication request
    public class GoogleAuthDto
    {
        public string IdToken { get; set; } = string.Empty;
    }
}