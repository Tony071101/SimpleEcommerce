using System.ComponentModel.DataAnnotations;

namespace SimpleEcommerce.Models.Dtos.User
{
    public class UserUpdateDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; } // Nullable nếu không bắt buộc

        // Admin có thể cập nhật vai trò (TÙY CHỌN, CẨN THẬN KHI EXPOSE)
        public List<string>? Roles { get; set; }
    }
}