using Expense_WEB.Data;
using Expense_WEB.DTOs;
using Expense_WEB.Models;
using Microsoft.EntityFrameworkCore;

namespace Expense_WEB.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly BudgetContext _context;

        public BudgetService(BudgetContext context)
        {
            _context = context;
        }

        public async Task<TokenDtos.PaginatedResponse<Budget>> GetBudgetsAsync(int userId, string role, int page, int limit)
        {
            var query = role == "ADMIN"
                ? _context.Budgets.AsQueryable()
                : _context.Budgets.Where(b => b.UserId == userId);

            var totalItems = await query.CountAsync();
            var skip = (page - 1) * limit;

            var budgets = await query
                .Skip(skip)
                .Take(limit)
                .ToListAsync();

            return new TokenDtos.PaginatedResponse<Budget>
            {
                Data = budgets,
                Pagination = new TokenDtos.PaginationInfo
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling((double)totalItems / limit),
                    TotalItems = totalItems,
                    ItemsPerPage = limit,
                    HasNext = skip + limit < totalItems,
                    HasPrev = page > 1
                }
            };
        }

        public async Task<Budget?> GetBudgetByIdAsync(int id, int userId, string role)
        {
            var budget = await _context.Budgets.FindAsync(id);

            if (budget == null) return null;
            if (role != "ADMIN" && budget.UserId != userId)
                return null;

            return budget;
        }

        public async Task<Budget> CreateBudgetAsync(TokenDtos.CreateBudgetDto dto, int userId)
        {
            var budget = new Budget
            {
                Name = dto.Name,
                Amount = dto.Amount,
                UserId = userId
            };

            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();

            return budget;
        }

        public async Task<Budget?> UpdateBudgetAsync(int id, TokenDtos.UpdateBudgetDto dto, int userId, string role)
        {
            var budget = await _context.Budgets.FindAsync(id);

            if (budget == null) return null;
            if (role != "ADMIN" && budget.UserId != userId)
                return null;

            if (!string.IsNullOrEmpty(dto.Name))
                budget.Name = dto.Name;

            if (dto.Amount.HasValue && dto.Amount > 0)
                budget.Amount = dto.Amount.Value;

            await _context.SaveChangesAsync();
            return budget;
        }

        public async Task<bool> DeleteBudgetAsync(int id, int userId, string role)
        {
            var budget = await _context.Budgets.FindAsync(id);

            if (budget == null) return false;
            if (role != "ADMIN" && budget.UserId != userId)
                return false;

            var expenses = await _context.Expenses.Where(e => e.BudgetId == id).ToListAsync();
            _context.Expenses.RemoveRange(expenses);

            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
