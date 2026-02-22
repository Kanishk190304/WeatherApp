namespace AuthenticationService.Application.DTOs
{
    // User data transfer object (without sensitive data)
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? ProfilePicture { get; set; }
        public string AuthProvider { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}