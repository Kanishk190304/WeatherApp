namespace AuthenticationService.Application.Interfaces
{
    // Contract for Google token validation
    public interface IGoogleAuthService
    {
        Task<GoogleUserInfo?> ValidateGoogleTokenAsync(string idToken);
    }

    // Google user information from token
    public class GoogleUserInfo
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
        public string GoogleId { get; set; } = string.Empty;
    }
}