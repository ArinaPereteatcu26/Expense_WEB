namespace Expense_WEB.DTOs;

public class TokenDtos
{
    public class TokenRequest
    {
        public string Role { get; set; } = "USER";
        public List<string>? Permissions { get; set; }
        public int UserId { get; set; } = 1;
    }

    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
        public string ExpiresIn { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new();
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
}