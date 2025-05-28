using Expense_WEB.DTOs;

namespace Expense_WEB.Controllers
{

 using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
 
    using System.Security.Claims;

    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BudgetsController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetsController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }
        
        /*Get paginated list of budgets*/
        [HttpGet]
        [Authorize(Policy = "RequireReadPermission")]
        public async Task<IActionResult> GetBudgets([FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var role = User.FindFirst("role")?.Value ?? "USER";
            
            var result = await _budgetService.GetBudgetsAsync(userId, role, page, limit);
            return Ok(result);
        }

     
      /*Get budget by ID*/
      
        [HttpGet("{id}")]
        [Authorize(Policy = "RequireReadPermission")]
        public async Task<IActionResult> GetBudget(int id)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var role = User.FindFirst("role")?.Value ?? "USER";
            
            var budget = await _budgetService.GetBudgetByIdAsync(id, userId, role);
            
            if (budget == null)
                return NotFound(new { error = "Budget not found" });
            
            return Ok(budget);
        }

       
       /*Create a new budget*/
        [HttpPost]
        [Authorize(Policy = "RequireWritePermission")]
        public async Task<IActionResult> CreateBudget([FromBody] TokenDtos.CreateBudgetDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name) || dto.Amount <= 0)
                return BadRequest(new { error = "Name and positive amount are required" });

            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            
            var budget = await _budgetService.CreateBudgetAsync(dto, userId);
            return CreatedAtAction(nameof(GetBudget), new { id = budget.Id }, budget);
        }

     
        [HttpPut("{id}")]
        [Authorize(Policy = "RequireWritePermission")]
        public async Task<IActionResult> UpdateBudget(int id, [FromBody] TokenDtos.UpdateBudgetDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var role = User.FindFirst("role")?.Value ?? "USER";
            
            var budget = await _budgetService.UpdateBudgetAsync(id, dto, userId, role);
            
            if (budget == null)
                return NotFound(new { error = "Budget not found or access denied" });
            
            return Ok(budget);
        }
        
        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireDeletePermission")]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var role = User.FindFirst("role")?.Value ?? "USER";
            
            var success = await _budgetService.DeleteBudgetAsync(id, userId, role);
            
            if (!success)
                return NotFound(new { error = "Budget not found or access denied" });
            
            return NoContent();
        }
    }
}