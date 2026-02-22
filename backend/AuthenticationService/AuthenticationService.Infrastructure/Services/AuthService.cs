using AuthenticationService.Application.DTOs;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace AuthenticationService.Infrastructure.Services
{
    // Implementation of IAuthService for authentication operations
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository userRepository,
            IJwtService jwtService,
            IGoogleAuthService googleAuthService,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _googleAuthService = googleAuthService;
            _logger = logger;
        }

        // Register a new user
        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            _logger.LogInformation("Registration attempt for email: {Email}", request.Email);

            try
            {
                // Check if user already exists
                if (await _userRepository.ExistsAsync(request.Email))
                {
                    _logger.LogWarning("Registration failed - Email already exists: {Email}", request.Email);
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

                _logger.LogInformation("User registered successfully: {Email}, UserId: {UserId}", user.Email, user.Id);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Registration successful",
                    Token = token,
                    Email = user.Email,
                    Role = user.Role.ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for email: {Email}", request.Email);
                throw;
            }
        }

        // Login existing user
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            _logger.LogInformation("Login attempt for email: {Email}", request.Email);

            try
            {
                // Find user by email
                var user = await _userRepository.GetByEmailAsync(request.Email);

                // Check if user exists
                if (user == null)
                {
                    _logger.LogWarning("Login failed - User not found: {Email}", request.Email);
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                // Check if user registered with Google
                if (user.AuthProvider == "Google")
                {
                    _logger.LogWarning("Login failed - User should use Google login: {Email}", request.Email);
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Please use Google login for this account"
                    };
                }

                // Verify password
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Login failed - Invalid password for: {Email}", request.Email);
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                // Generate JWT token
                var token = _jwtService.GenerateToken(user);

                _logger.LogInformation("User logged in successfully: {Email}, UserId: {UserId}", user.Email, user.Id);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    Email = user.Email,
                    Role = user.Role.ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", request.Email);
                throw;
            }
        }

        // Login with Google
        public async Task<AuthResponseDto> GoogleLoginAsync(GoogleAuthDto request)
        {
            _logger.LogInformation("Google login attempt");

            try
            {
                // Validate Google token
                var googleUser = await _googleAuthService.ValidateGoogleTokenAsync(request.IdToken);

                if (googleUser == null)
                {
                    _logger.LogWarning("Google login failed - Invalid token");
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Invalid Google token"
                    };
                }

                _logger.LogInformation("Google token validated for email: {Email}", googleUser.Email);

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
                    _logger.LogInformation("New user created from Google account: {Email}", googleUser.Email);
                }
                else if (user.AuthProvider != "Google")
                {
                    _logger.LogWarning("Google login failed - Email registered with different provider: {Email}", googleUser.Email);
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Email already registered. Please use regular login."
                    };
                }

                // Generate JWT token
                var token = _jwtService.GenerateToken(user);

                _logger.LogInformation("Google login successful: {Email}, UserId: {UserId}", user.Email, user.Id);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Google login successful",
                    Token = token,
                    Email = user.Email,
                    Role = user.Role.ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Google login");
                throw;
            }
        }
    }
}