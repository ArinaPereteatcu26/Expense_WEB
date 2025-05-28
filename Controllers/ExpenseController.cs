/*using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Expense_WEB.Data;
using Expense_WEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: api/Expenses
        [HttpGet]
        [Authorize(Roles = "ADMIN,VISITOR,WRITER")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses([FromQuery] int skip = 0, [FromQuery] int limit = 10)
        {
            return await _context.Expenses
                .Skip(skip)
                .Take(limit)
                .ToListAsync();
        }
        
        // GET: api/Expenses/5
        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN,VISITOR,WRITER")]
        public async Task<ActionResult<Expense>> GetExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            
            if (expense == null)
            {
                return NotFound();
            }
            
            return expense;
        }
        
        // POST: api/Expenses
        [HttpPost]
        [Authorize(Roles = "ADMIN,WRITER")]
        public async Task<ActionResult<Expense>> CreateExpense(Expense expense)
        {
            // Get user ID from JWT claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            expense.UserId = userId;
            
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense);
        }
        
        // PUT: api/Expenses/5
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,WRITER")]
        public async Task<IActionResult> UpdateExpense(int id, Expense expense)
        {
            if (id != expense.Id)
            {
                return BadRequest();
            }
            
            _context.Entry(expense).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            return NoContent();
        }
        
        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }
}*/