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
        private readonly IGoogleAuthService _googleAuthService;

        public AuthService(
            IUserRepository userRepository, 
            IJwtService jwtService,
            IGoogleAuthService googleAuthService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _googleAuthService = googleAuthService;
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
                Role = Role.User,
                AuthProvider = "Local"
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

            // Check if user registered with Google
            if (user.AuthProvider == "Google")
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Please use Google login for this account"
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

        // Login with Google
        public async Task<AuthResponseDto> GoogleLoginAsync(GoogleAuthDto request)
        {
            // Validate Google token
            var googleUser = await _googleAuthService.ValidateGoogleTokenAsync(request.IdToken);

            if (googleUser == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid Google token"
                };
            }

            // Check if user exists
            var user = await _userRepository.GetByEmailAsync(googleUser.Email);

            if (user == null)
            {
                // Create new user from Google account
                user = new User
                {
                    Email = googleUser.Email,
                    Name = googleUser.Name,
                    ProfilePicture = googleUser.Picture,
                    GoogleId = googleUser.GoogleId,
                    PasswordHash = string.Empty,
                    Role = Role.User,
                    AuthProvider = "Google"
                };

                await _userRepository.CreateAsync(user);
            }
            else if (user.AuthProvider != "Google")
            {
                // User exists but registered with different provider
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Email already registered. Please use regular login."
                };
            }

            // Generate JWT token
            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Google login successful",
                Token = token,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }
    }
}