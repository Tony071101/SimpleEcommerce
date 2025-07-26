using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerce_BackEnd.Data;
using SimpleEcommerce_BackEnd.Models.Dtos.Auth;
using SimpleEcommerce_BackEnd.Models.Dtos.User;
using SimpleEcommerce_BackEnd.Models.Entities;
using SimpleEcommerce_BackEnd.Services.Interfaces;

namespace SimpleEcommerce_BackEnd.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext dbContext; // Vẫn cần DbContext cho các mối quan hệ và query không liên quan trực tiếp đến Identity core
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        public UserService(ApplicationDbContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersWithRolesAsync()
        {
            var users = await userManager.Users.ToListAsync(); // Lấy tất cả người dùng
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user); // Lấy vai trò cho từng người dùng
                var userDto = new UserDto
                {
                    Id = user.Id,
                    Username = user.UserName!,
                    Email = user.Email!,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToList() // Gán danh sách vai trò
                };
                userDtos.Add(userDto);
            }
            return userDtos;
        }

        // Phương thức mới hoặc sửa đổi để lấy một người dùng CÙNG VỚI VAI TRÒ của họ
        public async Task<UserDto?> GetUserDtoByIdAsync(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return null;
            }

            var roles = await userManager.GetRolesAsync(user); // Lấy vai trò cho người dùng này
            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.UserName!,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList() // Gán danh sách vai trò
            };
            return userDto;
        }

        public async Task<(bool Success, string Message, User? User, List<string>? Roles)> AddUserByAdminAsync(RegisterDto registerDto, string[] roles)
        {
            var userExists = await userManager.FindByEmailAsync(registerDto.Email);
            if (userExists != null)
            {
                return (false, "Email is already registered.", null, null);
            }

            var user = new User
            {
                UserName = registerDto.Username,
                PasswordHash = registerDto.Password,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, $"User creation failed! {errors}", null, null);
            }

            if (roles != null && roles.Any())
            {
                var invalidRoles = new List<string>();
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        invalidRoles.Add(role);
                    }
                }

                if (invalidRoles.Any())
                {
                    // Rollback user creation or indicate partial success
                    // For simplicity, we'll return failure here if any role is invalid.
                    await userManager.DeleteAsync(user); // Rollback user creation
                    return (false, $"User creation failed: Invalid roles provided: {string.Join(", ", invalidRoles)}", null, null);
                }

                var addRolesResult = await userManager.AddToRolesAsync(user, roles);
                if (!addRolesResult.Succeeded)
                {
                    // If roles fail, consider rolling back user creation as well
                    await userManager.DeleteAsync(user); // Rollback
                    var errors = string.Join(", ", addRolesResult.Errors.Select(e => e.Description));
                    return (false, $"User created but failed to assign roles: {errors}", null, null);
                }
            }
            else
            {
                // Default to "Customer" role if no roles are specified
                if (await roleManager.RoleExistsAsync("Customer"))
                {
                    await userManager.AddToRoleAsync(user, "Customer");
                }
                else
                {
                    // Handle case where "Customer" role does not exist
                    // This might indicate an incomplete setup
                }
            }

            var currentRoles = (await userManager.GetRolesAsync(user)).ToList();
            return (true, "User created successfully.", user, currentRoles);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var userToDelete = await userManager.FindByIdAsync(id.ToString());
            if (userToDelete == null)
            {
                return false; // User not found
            }

            var result = await userManager.DeleteAsync(userToDelete);
            return result.Succeeded;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await userManager.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await userManager.FindByIdAsync(id.ToString());
        }

        public async Task<(bool Success, string Message, User? User, List<string>? Roles)> UpdateUserByAdminAsync(Guid id, UserUpdateDto userDto)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return (false, "User not found.", null, null);
            }

            var existingUserWithEmail = await userManager.FindByEmailAsync(userDto.Email);
            if (existingUserWithEmail != null && existingUserWithEmail.Id != user.Id)
            {
                return (false, "Email is already in use by another user.", null, null);
            }
            var existingUserWithUsername = await userManager.FindByNameAsync(userDto.Username);
            if (existingUserWithUsername != null && existingUserWithUsername.Id != user.Id)
            {
                return (false, "Username is already in use by another user.", null, null);
            }

            user.UserName = userDto.Username;
            user.Email = userDto.Email;
            user.PhoneNumber = userDto.PhoneNumber;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, $"Failed to update user: {errors}", null, null);
            }

            var updatedRoles = new List<string>();
            if (userDto.Roles != null)
            {
                var currentRoles = await userManager.GetRolesAsync(user);
                var rolesToRemove = currentRoles.Except(userDto.Roles);
                var rolesToAdd = userDto.Roles.Except(currentRoles);

                // Validate roles to add
                var invalidRolesToAdd = new List<string>();
                foreach (var role in rolesToAdd)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        invalidRolesToAdd.Add(role);
                    }
                }
                if (invalidRolesToAdd.Any())
                {
                    return (false, $"Update failed: Invalid roles provided: {string.Join(", ", invalidRolesToAdd)}", user, null);
                }

                await userManager.RemoveFromRolesAsync(user, rolesToRemove);
                await userManager.AddToRolesAsync(user, rolesToAdd);

                updatedRoles.AddRange(userDto.Roles); // Update the list of roles that were changed
            }
            else
            {
                updatedRoles.AddRange(await userManager.GetRolesAsync(user)); // Get current roles if DTO doesn't provide them
            }

            return (true, "User updated successfully.", user, updatedRoles);
        }

        public async Task<(bool Success, string Message)> UpdateUserPasswordByAdminAsync(Guid id, string newPassword)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return (false, "User not found.");
            }

            // Tạo token reset mật khẩu
            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
            // Đặt lại mật khẩu với token
            var result = await userManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, $"Failed to update password: {errors}");
            }
            return (true, "Password updated successfully.");
        }
    }
}