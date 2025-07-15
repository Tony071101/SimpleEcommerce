using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Extensions;
using SimpleEcommerce.Models;
using SimpleEcommerce.Models.Dtos.Address;
using SimpleEcommerce.Models.Entities;
using SimpleEcommerce.Services.Interfaces;

namespace SimpleEcommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService addressService;
        private readonly IMapper mapper;
        public AddressesController(IAddressService addressService, IMapper mapper)
        {
            this.addressService = addressService;
            this.mapper = mapper;
        }

        [HttpGet("all")] // Đường dẫn riêng biệt cho Admin
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<AddressResponseDto>>> GetAllAddresses()
        {
            var addresses = await addressService.GetAllAddressesAsync();
            var addressDtos = mapper.Map<IEnumerable<AddressResponseDto>>(addresses);
            return Ok(addressDtos);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AddressResponseDto>> GetAddressById(Guid id)
        {
            var address = await addressService.GetAddressByIdAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            var addressDto = mapper.Map<AddressResponseDto>(address);
            return Ok(addressDto);
        }

        [HttpGet("customer/{customerId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<AddressResponseDto>>> GetAddressesByCustomerId(Guid customerId)
        {
            var addresses = await addressService.GetAddressesByCustomerIdAsync(customerId);
            var addressDtos = mapper.Map<IEnumerable<AddressResponseDto>>(addresses);
            return Ok(addressDtos);
        }

        [HttpGet("my")] // Khách hàng chỉ có thể lấy địa chỉ của chính họ
        [Authorize(Roles = "Customer,Admin")] // Cả Customer và Admin đều có thể dùng để lấy địa chỉ của chính họ
        public async Task<ActionResult<IEnumerable<AddressResponseDto>>> GetMyAddresses()
        {
            var currentUserId = User.GetUserId();
            var addresses = await addressService.GetAddressesByCustomerIdAsync(currentUserId);
            var addressDtos = mapper.Map<IEnumerable<AddressResponseDto>>(addresses);
            return Ok(addressDtos);
        }

        [HttpPost]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<ActionResult<AddressResponseDto>> AddAddress([FromBody] CreateAddressDto createAddressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = User.GetUserId();
            var addressToAdd = mapper.Map<Address>(createAddressDto);

            try
            {
                // Pass currentUserId để service gán vào UserID của địa chỉ
                var newAddress = await addressService.AddAddressAsync(currentUserId, addressToAdd);
                var newAddressDto = mapper.Map<AddressResponseDto>(newAddress);

                return CreatedAtAction(nameof(GetAddressById), new { id = newAddress.AddressID }, newAddressDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> UpdateAddress(Guid id, [FromBody] UpdateAddressDto updateAddressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = User.GetUserId();

            var addressToUpdate = mapper.Map<Address>(updateAddressDto);
            // Đảm bảo AddressID được giữ nguyên
            addressToUpdate.AddressID = id;

            try
            {
                // Truyền userId để service kiểm tra quyền sở hữu
                var updatedAddress = await addressService.UpdateAddressAsync(id, currentUserId, addressToUpdate);

                if (updatedAddress == null)
                {
                    return NotFound("Address not found.");
                }

                var updatedAddressDto = mapper.Map<AddressResponseDto>(updatedAddress);
                return Ok(updatedAddressDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message); // Trả về 403 Forbidden nếu không có quyền
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            var currentUserId = User.GetUserId();

            try
            {
                // Truyền userId để service kiểm tra quyền sở hữu
                var deleteSuccess = await addressService.DeleteAddressAsync(id, currentUserId);
                return deleteSuccess ? NoContent() : NotFound("Failed to delete address.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message); // Trả về 403 Forbidden nếu không có quyền
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}