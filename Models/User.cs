using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Expense_WEB.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "USER";
    public List<string> Permissions { get; set; } = new();
}