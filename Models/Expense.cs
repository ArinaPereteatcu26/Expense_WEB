using System;
using System.ComponentModel.DataAnnotations;
namespace Expense_WEB.Models;

public class Expense
{
    [Key]
    public int Id { get; set; }
        
    [Required]
    public string Title { get; set; }
        
    [Required]
    public decimal Amount { get; set; }
        
    public DateTime Date { get; set; }
        
    public string Category { get; set; }
        
    public string Notes { get; set; }
        
    public string UserId { get; set; }
}