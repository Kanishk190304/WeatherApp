namespace AuthenticationService.Domain.Entities
{
    // Represents a user record in the database
    public class User
    {
        // Primary key
        public int Id { get; set; }

        // Login credentials - email and hashed password
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        // Authorization and audit fields
        public Role Role { get; set; } = Role.User;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}