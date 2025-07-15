using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Extensions;
using SimpleEcommerce.Models.Dtos.Order;
using SimpleEcommerce.Models.Entities;
using SimpleEcommerce.Services.Interfaces;

namespace SimpleEcommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        #region Customer
        [HttpPost]
        [Authorize(Roles = "Customer")] // Chỉ khách hàng mới có thể tạo đơn hàng
        public async Task<ActionResult<OrderResponseDto>> CreateOrder([FromBody] CreateOrderRequestDto createOrderDto)
        {
            var customerId = User.GetUserId();
            var orderToCreate = mapper.Map<Order>(createOrderDto);

            try
            {
                // Service sẽ xử lý logic nghiệp vụ: kiểm tra tồn kho, tính giá, lưu DB, xóa giỏ hàng
                var newOrder = await orderService.CreateOrderAsync(customerId, orderToCreate);

                // Ánh xạ Order Entity đã tạo sang OrderResponseDto trước khi trả về
                var newOrderDto = mapper.Map<OrderResponseDto>(newOrder);
                
                return CreatedAtAction(nameof(GetMyOrderById), new { orderId = newOrder.OrderID }, newOrderDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết hơn trong môi trường thực tế
                return StatusCode(500, $"An error occurred while creating the order: {ex.Message}");
            }
        }

        [HttpGet("my")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetMyOrders()
        {
            var customerId = User.GetUserId();
            var orders = await orderService.GetCustomerOrdersAsync(customerId);
            // Ánh xạ IEnumerable<Order> sang IEnumerable<OrderResponseDto>
            var orderDtos = mapper.Map<IEnumerable<OrderResponseDto>>(orders);
            return Ok(orderDtos);
        }

        [HttpGet("my/{orderId:guid}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<OrderResponseDto>> GetMyOrderById(Guid orderId)
        {
            var customerId = User.GetUserId();
            var order = await orderService.GetCustomerOrderByIdAsync(customerId, orderId);

            if (order == null)
            {
                return NotFound("Order not found or you do not have permission to view this order.");
            }

            // Ánh xạ Order Entity sang OrderResponseDto
            var orderDto = mapper.Map<OrderResponseDto>(order);
            return Ok(orderDto);
        }
        #endregion

        #region Admin
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAllOrders()
        {
            var orders = await orderService.GetAllOrdersAsync();
            // Ánh xạ IEnumerable<Order> sang IEnumerable<OrderResponseDto>
            var orderDtos = mapper.Map<IEnumerable<OrderResponseDto>>(orders);
            return Ok(orderDtos);
        }

        [HttpGet("{orderId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrderResponseDto>> GetOrderById(Guid orderId)
        {
            var order = await orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }
            var orderDto = mapper.Map<OrderResponseDto>(order);
            return Ok(orderDto);
        }

        [HttpPut("{orderId:guid}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromBody] UpdateOrderStatusDto updateStatusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedOrder = await orderService.UpdateOrderStatusAsync(orderId, updateStatusDto.NewStatus);
                if (updatedOrder == null)
                {
                    return NotFound("Order not found.");
                }
                var updatedOrderDto = mapper.Map<OrderResponseDto>(updatedOrder);
                return Ok(updatedOrderDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Ví dụ: trạng thái không hợp lệ
            }
            catch (Exception ex)
            {
                // Log lỗi
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        #endregion
    }
}