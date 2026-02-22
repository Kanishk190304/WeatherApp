using AuthenticationService.Application.DTOs;
using AuthenticationService.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.API.Controllers
{
    // API controller for user management (Admin only)
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserAdoRepository _userAdoRepository;

        public UsersController(IUserAdoRepository userAdoRepository)
        {
            _userAdoRepository = userAdoRepository;
        }

        // GET: api/users?pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Validate pagination parameters
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 100) pageSize = 100;

                // Get paginated users using ADO.NET stored procedure
                var (users, totalCount) = await _userAdoRepository.GetUsersWithPaginationAsync(pageNumber, pageSize);

                // Map to DTOs (exclude sensitive data)
                var userDtos = users.Select(u => new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Role = u.Role.ToString(),
                    Name = u.Name,
                    ProfilePicture = u.ProfilePicture,
                    AuthProvider = u.AuthProvider,
                    CreatedAt = u.CreatedAt
                });

                // Return paginated result
                var result = new PaginatedResultDto<UserDto>
                {
                    Data = userDtos,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving users", Error = ex.Message });
            }
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userAdoRepository.GetByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new { Message = $"User with ID {id} not found" });
                }

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = user.Role.ToString(),
                    Name = user.Name,
                    ProfilePicture = user.ProfilePicture,
                    AuthProvider = user.AuthProvider,
                    CreatedAt = user.CreatedAt
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving user", Error = ex.Message });
            }
        }
    }
}