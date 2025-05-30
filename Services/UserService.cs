using Expense_WEB.Controllers;

namespace Expense_WEB.Services
{



    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        // In a real app, you'd inject your database context here
        // private readonly BudgetContext _context;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> UserExists(string username)
        {
            // For demo purposes, always return false to allow registration
            // In production, check your database
            _logger.LogInformation($"Checking if user exists: {username}");
            await Task.Delay(1); // Simulate async operation
            return false;
        }

        public async Task<UserDto> CreateUser(RegisterRequest request)
        {
            _logger.LogInformation($"Creating user: {request.Username}");

            // In production, you'd:
            // 1. Hash the password
            // 2. Save to database
            // 3. Return the created user

            await Task.Delay(1); // Simulate async operation

            return new UserDto
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Role = "User",
                Permissions = new List<string> { "READ", "WRITE" }
            };
        }

        public async Task<UserDto?> ValidateUser(string username, string password)
        {
            // For demo purposes, accept any credentials
            // In production, validate against hashed passwords in database
            _logger.LogInformation($"Validating user: {username}");
            await Task.Delay(1); // Simulate async operation

            return new UserDto
            {
                Id = Guid.NewGuid(),
                Username = username,
                Role = "User",
                Permissions = new List<string> { "READ", "WRITE" }
            };
        }
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new();
    }
}