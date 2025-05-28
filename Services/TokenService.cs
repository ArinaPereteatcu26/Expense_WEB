using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Expense_WEB.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace Expense_WEB.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenDtos.TokenResponse GenerateToken(TokenDtos.TokenRequest request)
        {
            var validRoles = new[] { "ADMIN", "USER", "VISITOR" };
            if (!validRoles.Contains(request.Role))
            {
                throw new ArgumentException("Invalid role. Must be ADMIN, USER, or VISITOR");
            }

            var defaultPermissions = new Dictionary<string, List<string>>
            {
                ["ADMIN"] = new() { "READ", "WRITE", "DELETE", "MANAGE" },
                ["USER"] = new() { "READ", "WRITE" },
                ["VISITOR"] = new() { "READ" }
            };

            var permissions = request.Role == "ADMIN" 
                ? defaultPermissions["ADMIN"] 
                : request.Permissions ?? defaultPermissions.GetValueOrDefault(request.Role, new List<string> { "READ" });

            var secretKey = _configuration["JwtSettings:SecretKey"] ?? "YourSuperSecretKeyThatShouldBeChangedInProduction123!";
            var key = Encoding.ASCII.GetBytes(secretKey);

            var claims = new List<Claim>
            {
                new("userId", request.UserId.ToString()),
                new("role", request.Role),
                new("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permission", permission));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(1), // 1 minute expiration for demo
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenDtos.TokenResponse
            {
                Token = tokenHandler.WriteToken(token),
                ExpiresIn = "1m",
                Role = request.Role,
                Permissions = permissions
            };
        }
    }
}
