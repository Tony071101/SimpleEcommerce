using System.ComponentModel.DataAnnotations;

namespace SimpleEcommerce.Models.Dtos.Order
{
    public class CreateOrderRequestDto
    {
        [Required(ErrorMessage = "Order items are required.")]
        [MinLength(1, ErrorMessage = "At least one item is required in the order.")]
        public List<CreateOrderItemRequestDto> Items { get; set; } = new();

        public Guid? ShippingAddressId { get; set; }
    }

    public class CreateOrderItemRequestDto
    {
        [Required(ErrorMessage = "Product ID is required for an order item.")]
        public Guid ProductID { get; set; }

        [Required(ErrorMessage = "Quantity is required for an order item.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }
}