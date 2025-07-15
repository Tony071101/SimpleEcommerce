using System.ComponentModel.DataAnnotations;

namespace SimpleEcommerce.Models.Dtos.Auth
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")] // Đảm bảo mật khẩu nhập lại khớp
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;

        // Bạn có thể thêm các trường khác nếu cần cho thông tin đăng ký ban đầu
        public string? PhoneNumber { get; set; }
    }
}