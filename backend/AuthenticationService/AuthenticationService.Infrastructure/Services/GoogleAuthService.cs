using AuthenticationService.Application.Interfaces;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AuthenticationService.Infrastructure.Services
{
    // Implementation of IGoogleAuthService for Google token validation
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly string _clientId;
        private readonly ILogger<GoogleAuthService> _logger;

        public GoogleAuthService(IConfiguration configuration, ILogger<GoogleAuthService> logger)
        {
            _clientId = configuration["Google:ClientId"]!;
            _logger = logger;
        }

        // Validate Google ID token and extract user info
        public async Task<GoogleUserInfo?> ValidateGoogleTokenAsync(string idToken)
        {
            try
            {
                _logger.LogInformation("Validating Google token with Client ID: {ClientId}", _clientId);
                _logger.LogInformation("Token starts with: {TokenPrefix}...", idToken.Substring(0, Math.Min(50, idToken.Length)));
                _logger.LogInformation("Token length: {TokenLength}", idToken.Length);
                
                // Validate the token with Google
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _clientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                _logger.LogInformation("Token validated successfully for email: {Email}", payload.Email);
                _logger.LogInformation("Token details - Name: {Name}, Subject: {Subject}", payload.Name, payload.Subject);

                // Return user info from token
                return new GoogleUserInfo
                {
                    Email = payload.Email,
                    Name = payload.Name,
                    Picture = payload.Picture,
                    GoogleId = payload.Subject
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating Google token. Client ID: {ClientId}, Exception Type: {ExceptionType}, Message: {ExceptionMessage}", _clientId, ex.GetType().Name, ex.Message);
                return null;
            }
        }
    }
}