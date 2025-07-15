using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Models.Dtos.Auth;
using SimpleEcommerce.Models.Dtos.User;
using SimpleEcommerce.Services.Interfaces;

namespace SimpleEcommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            // Service already returns UserDto, no mapping needed here.
            var userDtos = await userService.GetAllUsersWithRolesAsync();
            return Ok(userDtos);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            // Service already returns UserDto, no mapping needed here.
            var userDto = await userService.GetUserDtoByIdAsync(id);
            if (userDto == null) return NotFound();

            return Ok(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] RegisterDto registerDto, [FromQuery] string[]? roles = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Pass roles to the service, provide an empty array if null
            var (success, message, newUser, newRoles) = await userService.AddUserByAdminAsync(registerDto, roles ?? Array.Empty<string>());
            if (!success || newUser == null)
            {
                return BadRequest(new { Message = message });
            }

            // Map the User entity from the service response to UserDto for the API response
            var newUserDto = mapper.Map<UserDto>(newUser);
            newUserDto.Roles = newRoles ?? new List<string>(); // Assign roles returned by the service

            return CreatedAtAction(nameof(GetUserById), new { id = newUserDto.Id }, newUserDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserUpdateDto userUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, message, updatedUser, updatedRoles) = await userService.UpdateUserByAdminAsync(id, userUpdateDto);

            if (!success || updatedUser == null)
            {
                return BadRequest(new { Message = message });
            }

            // Map the User entity from the service response to UserDto
            var updatedUserDto = mapper.Map<UserDto>(updatedUser);
            updatedUserDto.Roles = updatedRoles ?? new List<string>(); // Assign roles returned by the service

            return Ok(updatedUserDto);
        }

        [HttpPut("{id:guid}/password")]
        public async Task<IActionResult> UpdateUserPassword(Guid id, [FromBody] UserPasswordUpdateDto passwordUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, message) = await userService.UpdateUserPasswordByAdminAsync(id, passwordUpdateDto.NewPassword);
            if (!success)
            {
                return BadRequest(new { Message = message });
            }
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var success = await userService.DeleteUserAsync(id); // Renamed method
            if (!success)
            {
                return NotFound("User not found or could not be deleted.");
            }
            return NoContent();
        }
    }
}