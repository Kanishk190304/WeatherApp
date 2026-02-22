using AuthenticationService.Domain.Entities;

namespace AuthenticationService.Domain.Interfaces
{
    // Contract for ADO.NET user repository operations
    public interface IUserAdoRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task<bool> ExistsAsync(string email);
        Task<(IEnumerable<User> Users, int TotalCount)> GetUsersWithPaginationAsync(int pageNumber, int pageSize);
    }
}