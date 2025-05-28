using Expense_WEB.DTOs;

namespace Expense_WEB
{
    public interface ITokenService
    {
        TokenDtos.TokenResponse GenerateToken(TokenDtos.TokenRequest request);
    }
}