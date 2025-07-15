using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleEcommerce.Models.Dtos.Auth
{
    public class AuthResponseDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>(); // Các vai trò của người dùng
        public string Token { get; set; } = string.Empty; // JWT Token
        public DateTime Expiration { get; set; } // Thời gian hết hạn của token
    }
}