using Expense_WEB.Models;

namespace Expense_WEB.Data;


using Microsoft.EntityFrameworkCore;


public class BudgetContext : DbContext
{
    public BudgetContext(DbContextOptions<BudgetContext> options) : base(options) { }

    public DbSet<Budget> Budgets { get; set; }
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Budget>()
            .Property(b => b.Amount)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Budget>()
            .Property(b => b.Spent)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Expense>()
            .Property(e => e.Amount)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Expense>()
            .HasOne(e => e.Budget)
            .WithMany(b => b.Expenses)
            .HasForeignKey(e => e.BudgetId);
    }
}

public static class SeedData
{
    public static void Initialize(BudgetContext context)
    {
        context.Database.EnsureCreated();

        if (!context.Budgets.Any())
        {
            context.Budgets.AddRange(
                new Budget { Name = "Groceries", Amount = 500, UserId = 1 },
                new Budget { Name = "Transportation", Amount = 200, UserId = 1 },
                new Budget { Name = "Entertainment", Amount = 150, UserId = 2 }
            );
            context.SaveChanges();
        }
    }
}