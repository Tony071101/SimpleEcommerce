using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce_BackEnd.Extensions;
using SimpleEcommerce_BackEnd.Models.Dtos.Cart;
using SimpleEcommerce_BackEnd.Services.Interfaces;

namespace SimpleEcommerce_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")]
    public class CartsController : ControllerBase
    {
        private readonly ICartService cartService;
        private readonly IProductService productService; // Cần thiết để kiểm tra tồn kho và lấy thông tin sản phẩm
        private readonly IMapper mapper;

        public CartsController(ICartService cartService, IProductService productService, IMapper mapper)
        {
            this.cartService = cartService;
            this.productService = productService;
            this.mapper = mapper;
        }

        [HttpGet("my")]
        public async Task<ActionResult<CartResponseDto>> GetMyCart()
        {
            var customerId = User.GetUserId();
            // Lấy các mặt hàng trong giỏ hàng (đã được bao gồm thông tin Product)
            var cartItems = await cartService.GetCartByCustomerIdAsync(customerId);

            if (!cartItems.Any())
            {
                // Trả về giỏ hàng rỗng nếu không có mặt hàng nào
                return Ok(new CartResponseDto { CustomerID = customerId, Items = new List<CartItemResponseDto>(), TotalAmount = 0 });
            }

            // Ánh xạ danh sách các Cart Entity sang danh sách CartItemResponseDto
            var cartItemDtos = mapper.Map<List<CartItemResponseDto>>(cartItems);
            var totalAmount = cartItemDtos.Sum(item => item.SubTotal);

            var cartResponse = new CartResponseDto
            {
                CustomerID = customerId,
                Items = cartItemDtos,
                TotalAmount = totalAmount
            };

            return Ok(cartResponse);
        }

        [HttpPost("add-or-update")]
        public async Task<IActionResult> AddOrUpdateCartItem([FromBody] CartItemRequestDto requestDto)
        {
            var customerId = User.GetUserId(); // Lấy CustomerID từ claims của người dùng đã xác thực

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Không cần kiểm tra cartDto.CustomerID != customerId nữa vì requestDto không chứa CustomerID

            if (requestDto.Quantity <= 0)
            {
                // Nếu số lượng là 0 hoặc âm, coi đó là yêu cầu xóa
                var removed = await cartService.RemoveCartItemAsync(customerId, requestDto.ProductID);
                return removed ? Ok("Item quantity set to 0 and removed from cart.") : NotFound("Product not found in cart to remove.");
            }

            var product = await productService.GetProductByIdSync(requestDto.ProductID);
            if (product == null)
            {
                return NotFound($"Product with ID {requestDto.ProductID} not found.");
            }

            // Kiểm tra tồn kho
            // requestDto.Quantity ở đây là số lượng MỚI mà người dùng muốn có trong giỏ hàng
            if (product.ProductStock < requestDto.Quantity)
            {
                return BadRequest($"Insufficient stock for product '{product.ProductName}'. Available: {product.ProductStock}, Requested: {requestDto.Quantity}");
            }

            // Gọi service để thêm mới hoặc cập nhật số lượng
            var updatedCartItem = await cartService.AddOrUpdateCartItemAsync(customerId, requestDto.ProductID, requestDto.Quantity);

            // Kiểm tra kết quả từ service
            if (updatedCartItem == null && requestDto.Quantity > 0) // Nếu service trả về null nhưng số lượng > 0, có thể là lỗi
            {
                return StatusCode(500, "An error occurred while adding/updating cart item.");
            }

            return Ok("Cart item added/updated successfully.");
        }

        [HttpDelete("remove/{productId:guid}")]
        public async Task<IActionResult> RemoveItemFromCart(Guid productId)
        {
            var customerId = User.GetUserId();
            var success = await cartService.RemoveCartItemAsync(customerId, productId);
            return success ? NoContent() : NotFound("Product not found in your cart.");
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearMyCart()
        {
            var customerId = User.GetUserId();
            await cartService.ClearCartAsync(customerId);
            return NoContent();
        }
    }
}