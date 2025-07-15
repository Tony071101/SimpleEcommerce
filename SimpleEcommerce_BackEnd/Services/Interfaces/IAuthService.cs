using SimpleEcommerce.Models.Dtos.Auth;

namespace SimpleEcommerce.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterUserAsync(RegisterDto registerDto);
        Task<(bool Success, string Token, DateTime Expiration, AuthResponseDto? AuthData)> LoginUserAsync(string email, string password);
    }
}