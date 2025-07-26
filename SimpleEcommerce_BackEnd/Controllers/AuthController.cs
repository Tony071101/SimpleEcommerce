using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce_BackEnd.Models.Dtos.Auth;
using SimpleEcommerce_BackEnd.Services.Interfaces;

namespace SimpleEcommerce_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IMapper mapper; // Cần IMapper nếu bạn có ánh xạ từ RegisterDto sang User Entity

        public AuthController(IAuthService authService, IMapper mapper)
        {
            this.authService = authService;
            this.mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Gọi service với RegisterDto
            var (success, message) = await authService.RegisterUserAsync(registerDto);

            if (success)
            {
                // Trả về thông báo thành công
                return Ok(new { Message = message });
            }
            else
            {
                return BadRequest(new { Message = message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, _, _, authData) = await authService.LoginUserAsync(loginDto.Email, loginDto.Password);

            if (success && authData != null)
            {
                return Ok(authData);
            }
            else
            {
                return Unauthorized(new { Message = "Invalid login credentials." });
            }
        }
    }
}