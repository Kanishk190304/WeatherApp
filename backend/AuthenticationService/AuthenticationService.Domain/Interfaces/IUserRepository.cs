using AuthenticationService.Domain.Entities;

namespace AuthenticationService.Domain.Interfaces
{
    // Contract for user data access operations
    public interface IUserRepository
    {
        // Read operations - retrieve user by ID or email
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);

        // Write operation - create new user
        Task<User> CreateAsync(User user);

        // Validation operation - check if email already exists
        Task<bool> ExistsAsync(string email);
    }
}