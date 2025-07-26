namespace SimpleEcommerce_BackEnd.Models.Dtos.User
{
    public class UserDto
    {
        public Guid Id { get; set; } // Đây là Id của IdentityUser
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public List<string> Roles { get; set; } = new(); // Danh sách vai trò của người dùng
    }
}