using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.Interfaces;
using AuthenticationService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Infrastructure.Repositories
{
    // Implementation of IUserRepository using Entity Framework
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get user by ID
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        // Get user by email
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        // Create new user
        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // Check if email already exists
        public async Task<bool> ExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}