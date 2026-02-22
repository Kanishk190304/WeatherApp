using System.Data;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AuthenticationService.Infrastructure.Repositories
{
    // Implementation of IUserAdoRepository using ADO.NET and stored procedures
    public class UserAdoRepository : IUserAdoRepository
    {
        private readonly string _connectionString;

        public UserAdoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        // Get user by ID using stored procedure
        public async Task<User?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("sp_GetUserById", connection);
                
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return MapUserFromReader(reader);
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error while getting user by ID: {ex.Message}", ex);
            }
        }

        // Get user by email using stored procedure
        public async Task<User?> GetByEmailAsync(string email)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("sp_GetUserByEmail", connection);
                
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Email", email);

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return MapUserFromReader(reader);
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error while getting user by email: {ex.Message}", ex);
            }
        }

        // Create user using stored procedure
        public async Task<User> CreateAsync(User user)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("sp_CreateUser", connection);
                
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash ?? string.Empty);
                command.Parameters.AddWithValue("@Role", (int)user.Role);
                command.Parameters.AddWithValue("@GoogleId", (object?)user.GoogleId ?? DBNull.Value);
                command.Parameters.AddWithValue("@Name", (object?)user.Name ?? DBNull.Value);
                command.Parameters.AddWithValue("@ProfilePicture", (object?)user.ProfilePicture ?? DBNull.Value);
                command.Parameters.AddWithValue("@AuthProvider", user.AuthProvider);

                var outputParam = new SqlParameter("@NewId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputParam);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                user.Id = (int)outputParam.Value;
                return user;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error while creating user: {ex.Message}", ex);
            }
        }

        // Check if user exists using stored procedure
        public async Task<bool> ExistsAsync(string email)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("sp_CheckUserExists", connection);
                
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Email", email);

                var outputParam = new SqlParameter("@Exists", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputParam);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                return (bool)outputParam.Value;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error while checking user exists: {ex.Message}", ex);
            }
        }

        // Get users with pagination using stored procedure
        public async Task<(IEnumerable<User> Users, int TotalCount)> GetUsersWithPaginationAsync(int pageNumber, int pageSize)
        {
            try
            {
                var users = new List<User>();

                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("sp_GetUsersWithPagination", connection);
                
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PageNumber", pageNumber);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                var totalCountParam = new SqlParameter("@TotalCount", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(totalCountParam);

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    users.Add(MapUserFromReader(reader));
                }

                var totalCount = (int)totalCountParam.Value;
                return (users, totalCount);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error while getting users with pagination: {ex.Message}", ex);
            }
        }

        // Helper method to map SqlDataReader to User entity
        private static User MapUserFromReader(SqlDataReader reader)
        {
            return new User
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                Role = (Role)reader.GetInt32(reader.GetOrdinal("Role")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                GoogleId = reader.IsDBNull(reader.GetOrdinal("GoogleId")) ? null : reader.GetString(reader.GetOrdinal("GoogleId")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                ProfilePicture = reader.IsDBNull(reader.GetOrdinal("ProfilePicture")) ? null : reader.GetString(reader.GetOrdinal("ProfilePicture")),
                AuthProvider = reader.GetString(reader.GetOrdinal("AuthProvider"))
            };
        }
    }
}