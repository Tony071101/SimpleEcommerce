using SimpleEcommerce_BackEnd.Models.Dtos.Auth;

namespace SimpleEcommerce_BackEnd.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterUserAsync(RegisterDto registerDto);
        Task<(bool Success, string Token, DateTime Expiration, AuthResponseDto? AuthData)> LoginUserAsync(string email, string password);
    }
}