using System.ComponentModel.DataAnnotations;

namespace SimpleEcommerce_BackEnd.Models.Dtos.Cart
{
    public class CartItemRequestDto
    {
        [Required(ErrorMessage = "Product ID is required.")]
        public Guid ProductID { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative number.")]
        public int Quantity { get; set; }
    }
}