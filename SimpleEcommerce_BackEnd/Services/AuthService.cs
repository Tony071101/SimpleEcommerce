using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SimpleEcommerce_BackEnd.Models.Dtos.Auth;
using SimpleEcommerce_BackEnd.Models.Entities;
using SimpleEcommerce_BackEnd.Services.Interfaces;

namespace SimpleEcommerce_BackEnd.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole<Guid>> roleManager; // Thêm RoleManager để kiểm tra role "Customer"

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, RoleManager<IdentityRole<Guid>> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager; // Khởi tạo RoleManager
        }

        public async Task<(bool Success, string Token, DateTime Expiration, AuthResponseDto? AuthData)> LoginUserAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return (false, string.Empty, DateTime.MinValue, null); // User not found
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
            {
                return (false, string.Empty, DateTime.MinValue, null); // Login failed
            }

            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var tokenExpirationInMinutes = Convert.ToDouble(configuration["Jwt:TokenValidityInMinutes"]);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddMinutes(tokenExpirationInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var authResponse = new AuthResponseDto
            {
                UserId = user.Id.ToString(),
                Username = user.UserName!,
                Email = user.Email!,
                Roles = userRoles.ToList(),
                Token = tokenString,
                Expiration = token.ValidTo
            };

            return (true, tokenString, token.ValidTo, authResponse);
        }

        // Changed signature to take RegisterDto
        public async Task<(bool Success, string Message)> RegisterUserAsync(RegisterDto registerDto)
        {
            var user = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber, // Map PhoneNumber from DTO
                CreatedDate = DateTime.UtcNow // Set creation date
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                // Assign "Customer" role to newly registered user
                if (await roleManager.RoleExistsAsync("Customer"))
                {
                    await userManager.AddToRoleAsync(user, "Customer");
                }
                else
                {
                    // Log: "Customer" role does not exist. Please create it.
                    // This scenario should ideally be prevented by proper role seeding.
                }
                return (true, "User registered successfully.");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, $"Registration failed: {errors}");
            }
        }
    }
}