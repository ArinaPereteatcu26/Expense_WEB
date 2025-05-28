using Expense_WEB.DTOs;
using Expense_WEB.Models;

namespace Expense_WEB;

public interface IExpenseService
{
    Task<TokenDtos.PaginatedResponse<Expense>> GetExpensesAsync(int userId, string role, int page, int limit, int? budgetId);
    Task<Expense?> CreateExpenseAsync(TokenDtos.CreateExpenseDto dto, int userId, string role);
    Task<bool> DeleteExpenseAsync(int id, int userId, string role);
}