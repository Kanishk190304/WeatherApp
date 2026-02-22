namespace AuthenticationService.Application.DTOs
{
    /// <summary>
    /// Data transfer object for Google login request (frontend compatible format)
    /// </summary>
    public class GoogleLoginRequestDto
    {
        /// <summary>
        /// Google ID token from Google Sign-In
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
