﻿using System;
using System.ComponentModel.DataAnnotations;
namespace Expense_WEB.Models;

public class Expense
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int BudgetId { get; set; }
    public Budget Budget { get; set; } = null!;
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
