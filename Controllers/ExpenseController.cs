using Expense_WEB.DTOs;

namespace Expense_WEB.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;


    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpensesController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }
        
        [HttpGet]
        [Authorize(Policy = "RequireReadPermission")]
        public async Task<IActionResult> GetExpenses([FromQuery] int page = 1, [FromQuery] int limit = 10, [FromQuery] int? budgetId = null)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var role = User.FindFirst("role")?.Value ?? "USER";
            
            var result = await _expenseService.GetExpensesAsync(userId, role, page, limit, budgetId);
            return Ok(result);
        }
        
        [HttpPost]
        [Authorize(Policy = "RequireWritePermission")]
        public async Task<IActionResult> CreateExpense([FromBody] TokenDtos.CreateExpenseDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name) || dto.Amount <= 0 || dto.BudgetId <= 0)
                return BadRequest(new { error = "Name, positive amount, and budgetId are required" });

            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var role = User.FindFirst("role")?.Value ?? "USER";
            
            var expense = await _expenseService.CreateExpenseAsync(dto, userId, role);
            
            if (expense == null)
                return NotFound(new { error = "Budget not found or access denied" });
            
            return CreatedAtAction(nameof(GetExpenses), new { id = expense.Id }, expense);
        }
        
        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireDeletePermission")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var role = User.FindFirst("role")?.Value ?? "USER";
            
            var success = await _expenseService.DeleteExpenseAsync(id, userId, role);
            
            if (!success)
                return NotFound(new { error = "Expense not found or access denied" });
            
            return NoContent();
        }
    }
}