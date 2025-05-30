namespace Expense_WEB.DTOs;

public class TokenDtos
{
    public class TokenRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string[] Permissions { get; set; } = Array.Empty<string>();
    }

    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
        public string TokenType { get; set; } = "Bearer";
        public string ExpiresIn { get; set; } = string.Empty;
    }

    public class CreateBudgetDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class UpdateBudgetDto
    {
        public string? Name { get; set; }
        public decimal? Amount { get; set; }
    }

    public class CreateExpenseDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int BudgetId { get; set; }
    }

    public class PaginatedResponse<T>
    {
        public List<T> Data { get; set; } = new();
        public PaginationInfo Pagination { get; set; } = new();
    }

    public class PaginationInfo
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrev { get; set; }
    }
    
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}