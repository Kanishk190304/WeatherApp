using AuthenticationService.Application.DTOs;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.Interfaces;

namespace AuthenticationService.Infrastructure.Services
{
    // Implementation of IAuthService for authentication operations
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        // Register a new user
        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            // Check if user already exists
            if (await _userRepository.ExistsAsync(request.Email))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Email already registered"
                };
            }

            // Create new user with hashed password
            var user = new User
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = Role.User
            };

            // Save user to database
            await _userRepository.CreateAsync(user);

            // Generate JWT token
            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Registration successful",
                Token = token,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        // Login existing user
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            // Find user by email
            var user = await _userRepository.GetByEmailAsync(request.Email);

            // Check if user exists
            if (user == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            // Generate JWT token
            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Login successful",
                Token = token,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }
    }
}