using System.ComponentModel.DataAnnotations;

namespace SimpleEcommerce_BackEnd.Models.Dtos.Order
{
    public class UpdateOrderStatusDto
    {
        [Required(ErrorMessage = "New status is required.")]
        public string NewStatus { get; set; } = string.Empty;
    }
}