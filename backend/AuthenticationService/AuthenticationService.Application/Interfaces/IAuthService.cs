using AuthenticationService.Application.DTOs;

namespace AuthenticationService.Application.Interfaces
{
    // Contract for authentication operations
    public interface IAuthService
    {
        // Register a new user
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);

        // Login existing user
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    }
}