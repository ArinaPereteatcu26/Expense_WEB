using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Expense_WEB.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
    public List<string> Permissions { get; set; }
}