using AuthenticationService.Application.Interfaces;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace AuthenticationService.Infrastructure.Services
{
    // Implementation of IGoogleAuthService for Google token validation
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly string _clientId;

        public GoogleAuthService(IConfiguration configuration)
        {
            _clientId = configuration["Google:ClientId"]!;
        }

        // Validate Google ID token and extract user info
        public async Task<GoogleUserInfo?> ValidateGoogleTokenAsync(string idToken)
        {
            try
            {
                // Validate the token with Google
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _clientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                // Return user info from token
                return new GoogleUserInfo
                {
                    Email = payload.Email,
                    Name = payload.Name,
                    Picture = payload.Picture,
                    GoogleId = payload.Subject
                };
            }
            catch
            {
                return null;
            }
        }
    }
}