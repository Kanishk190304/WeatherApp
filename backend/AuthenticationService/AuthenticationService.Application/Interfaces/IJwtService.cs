using AuthenticationService.Domain.Entities;

namespace AuthenticationService.Application.Interfaces
{
    // Contract for JWT token operations
    public interface IJwtService
    {
        // Generate JWT token for authenticated user
        string GenerateToken(User user);
    }
}