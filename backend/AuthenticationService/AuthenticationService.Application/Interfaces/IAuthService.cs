using AuthenticationService.Application.DTOs;

namespace AuthenticationService.Application.Interfaces
{
    // Contract for authentication operations
    public interface IAuthService
    {
        // Standard authentication
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);

        // Federated authentication
        Task<AuthResponseDto> GoogleLoginAsync(GoogleAuthDto request);
    }
}