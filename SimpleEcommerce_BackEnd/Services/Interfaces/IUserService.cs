using SimpleEcommerce_BackEnd.Models.Dtos.Auth;
using SimpleEcommerce_BackEnd.Models.Dtos.User;
using SimpleEcommerce_BackEnd.Models.Entities;

namespace SimpleEcommerce_BackEnd.Services.Interfaces
{
    public interface IUserService
    {
        // Methods primarily returning Entities (can be used internally by other services or for specific scenarios)
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(Guid id);

        // Methods returning DTOs for API consumption (more common for controllers)
        Task<IEnumerable<UserDto>> GetAllUsersWithRolesAsync();
        Task<UserDto?> GetUserDtoByIdAsync(Guid id);

        // Admin Operations for User Management
        Task<(bool Success, string Message, User? User, List<string>? Roles)> AddUserByAdminAsync(RegisterDto registerDto, string[] roles);
        Task<(bool Success, string Message, User? User, List<string>? Roles)> UpdateUserByAdminAsync(Guid id, UserUpdateDto userDto);
        Task<(bool Success, string Message)> UpdateUserPasswordByAdminAsync(Guid id, string newPassword);

        Task<bool> DeleteUserAsync(Guid id);
    }
}