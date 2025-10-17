using Microsoft.AspNetCore.Mvc;
using PessoaAPI.DTOs;
using PessoaAPI.Services;

namespace PessoaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JWTService _jwtService;

        public AuthController(JWTService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_jwtService.ValidateUser(loginDTO.Username, loginDTO.Password))
            {
                var token = _jwtService.GenerateToken(loginDTO.Username);
                return Ok(new TokenResponseDTO
                {
                    Token = token,
                    ExpiresAt = DateTime.Now.AddHours(1)
                });
            }

            return Unauthorized(new { message = "Credenciais inválidas" });
        }
    }
}

