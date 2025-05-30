// Create a new file: Services/IUserService.cs
using Expense_WEB.Controllers;

namespace Expense_WEB.Services
{
    public interface IUserService
    {
        Task<bool> UserExists(string username);
        Task<UserDto> CreateUser(RegisterRequest request);
        Task<UserDto?> ValidateUser(string username, string password);
    }

}