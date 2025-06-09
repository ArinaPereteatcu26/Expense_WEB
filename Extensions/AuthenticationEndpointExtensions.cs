using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Expense_WEB.Models;

namespace Expense_WEB.Extensions
{
    public static class AuthenticationEndpointsExtensions
    {
        public static WebApplication ConfigureAuthEndpoints(this WebApplication app, IConfiguration configuration)
        {
            // Signup endpoint
            app.MapPost("/api/signup", async (
                UserManager<AppUser> userManager,
                [FromBody] UserRegistrationModel userRegistrationModel
            ) =>
            {
                try
                {
                    var existingUser = await userManager.FindByEmailAsync(userRegistrationModel.Email);
                    if (existingUser != null)
                    {
                        return Results.BadRequest(new { Error = "User already exists" });
                    }

                    AppUser user = new AppUser()
                    {
                        Email = userRegistrationModel.Email,
                        UserName = userRegistrationModel.Email,
                        FullName = userRegistrationModel.FullName,
                    };

                    var result = await userManager.CreateAsync(user, userRegistrationModel.Password);

                    if (result.Succeeded)
                        return Results.Ok(new { succeeded = true, message = "User created successfully", userId = user.Id });
                    else
                        return Results.BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"An error occurred: {ex.Message}");
                }
            });

            // Signin endpoint
            app.MapPost("/api/signin", async (
                UserManager<AppUser> userManager,
                [FromBody] LoginModel loginModel) =>
            {
                var user = await userManager.FindByEmailAsync(loginModel.Email);
                if (user != null && await userManager.CheckPasswordAsync(user, loginModel.Password))
                {
                    var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:JWTSecret"]!));
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim("UserID", user.Id.ToString()),
                        }),
                        Expires = DateTime.UtcNow.AddSeconds(60),
                        SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256Signature)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);
                    return Results.Ok(new { token });
                }
                return Results.BadRequest(new { message = "Invalid login or password" });
            });

            return app;
        }
    }
}