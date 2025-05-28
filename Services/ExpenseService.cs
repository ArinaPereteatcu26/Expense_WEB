using Expense_WEB.Data;
using Expense_WEB.DTOs;
using Expense_WEB.Models;
using Microsoft.EntityFrameworkCore;

namespace Expense_WEB.Services
{

public class ExpenseService : IExpenseService
{
    private readonly BudgetContext _context;

        public ExpenseService(BudgetContext context)
        {
            _context = context;
        }

        public async Task<TokenDtos.PaginatedResponse<Expense>> GetExpensesAsync(int userId, string role, int page, int limit, int? budgetId)
        {
            var query = role == "ADMIN" 
                ? _context.Expenses.AsQueryable()
                : _context.Expenses.Where(e => e.UserId == userId);

            if (budgetId.HasValue)
                query = query.Where(e => e.BudgetId == budgetId.Value);

            var totalItems = await query.CountAsync();
            var skip = (page - 1) * limit;
            
            var expenses = await query
                .Skip(skip)
                .Take(limit)
                .Include(e => e.Budget)
                .ToListAsync();

            return new TokenDtos.PaginatedResponse<Expense>
            {
                Data = expenses,
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

        public async Task<Expense?> CreateExpenseAsync(TokenDtos.CreateExpenseDto dto, int userId, string role)
        {
            var budget = await _context.Budgets.FindAsync(dto.BudgetId);
            if (budget == null) return null;
            
            if (role != "ADMIN" && budget.UserId != userId)
                return null;

            var expense = new Expense
            {
                Name = dto.Name,
                Amount = dto.Amount,
                BudgetId = dto.BudgetId,
                UserId = userId
            };

            _context.Expenses.Add(expense);
            
            // Update budget spent amount
            budget.Spent += dto.Amount;
            
            await _context.SaveChangesAsync();
            
            return expense;
        }

        public async Task<bool> DeleteExpenseAsync(int id, int userId, string role)
        {
            var expense = await _context.Expenses
                .Include(e => e.Budget)
                .FirstOrDefaultAsync(e => e.Id == id);
            
            if (expense == null) return false;
            
            if (role != "ADMIN" && expense.UserId != userId)
                return false;

            // Update budget spent amount
            expense.Budget.Spent -= expense.Amount;
            
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}