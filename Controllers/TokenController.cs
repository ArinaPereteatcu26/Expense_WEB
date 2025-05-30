using Expense_WEB.DTOs;

namespace Expense_WEB.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    
    
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        
        [HttpPost]
        public IActionResult GenerateToken([FromBody] TokenDtos.TokenRequest request)
        {
            try
            {
                var response = _tokenService.GenerateToken(request);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

}

