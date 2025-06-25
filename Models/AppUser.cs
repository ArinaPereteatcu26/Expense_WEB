using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Expense_WEB.Models
{

public class AppUser:IdentityUser
{
   [PersonalData]
   [Column(TypeName = "nvarchar(100)")]
   public string FullName { get; set; }
   
   [PersonalData]
   [Column(TypeName = "nvarchar(10)")]
   public string Gender { get; set; } 
   
   [PersonalData]
   public DateOnly DOB { get; set; }
   
   [PersonalData]
   public int? LibraryID { get; set; }
}
}