using Microsoft.AspNetCore.Mvc;
using Expense_WEB.Services;
using System.ComponentModel.DataAnnotations;
using Expense_WEB.DTOs;

namespace Expense_WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
        

        public AuthController(ITokenService tokenService, ILogger<AuthController> logger, IUserService userService)
        {
            _tokenService = tokenService;
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                
                var user = new
                {
                    Id = Guid.NewGuid(),
                    request.Username,
                    Role = request.Role ?? "User", // Default role
                    Permissions = request.Permissions ?? new[] { "READ", "WRITE" }
                };

                var tokenResponse = _tokenService.GenerateToken(new TokenDtos.TokenRequest
                {
                    Username = user.Username,
                    Role = user.Role,
                    Permissions = user.Permissions
                });

                return Ok(new AuthResponse
                {
                    Token = tokenResponse.Token,
                    TokenType = tokenResponse.TokenType,
                    ExpiresIn = tokenResponse.ExpiresIn,
                    User = new { 
                        user.Username, 
                        user.Role,
                        user.Permissions 
                    },
                    Message = "Registration successful"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed");
                return StatusCode(500, new { message = "Registration failed", error = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var tokenResponse = _tokenService.GenerateToken(new TokenDtos.TokenRequest
                {
                    Username = request.Username,
                    Role = request.Role,
                    Permissions = request.Permissions ?? new[] { "READ", "WRITE" }
                });

                return Ok(new AuthResponse
                {
                    Token = tokenResponse.Token,
                    TokenType = tokenResponse.TokenType,
                    ExpiresIn = tokenResponse.ExpiresIn,
                    User = new { 
                        request.Username, 
                        request.Role,
                        request.Permissions 
                    },
                    Message = "Login successful"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed");
                return StatusCode(500, new { message = "Login failed", error = ex.Message });
            }
        }
    }

    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        public string? Role { get; set; }
        public string[]? Permissions { get; set; }
        
        public string Password { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        public string? Role { get; set; }
        public string[]? Permissions { get; set; }
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string TokenType { get; set; } = "Bearer";
        public string ExpiresIn { get; set; } = string.Empty;
        public object? User { get; set; }
        public string? Message { get; set; }
    }
}