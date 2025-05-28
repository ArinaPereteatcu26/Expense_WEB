using Expense_WEB.DTOs;
using Expense_WEB.Models;

namespace Expense_WEB
{
    public interface IBudgetService
    {
        Task<TokenDtos.PaginatedResponse<Budget>> GetBudgetsAsync(int userId, string role, int page, int limit);
        Task<Budget?> GetBudgetByIdAsync(int id, int userId, string role);
        Task<Budget> CreateBudgetAsync(TokenDtos.CreateBudgetDto dto, int userId);
        Task<Budget?> UpdateBudgetAsync(int id, TokenDtos.UpdateBudgetDto dto, int userId, string role);
        Task<bool> DeleteBudgetAsync(int id, int userId, string role);
        
    }
}