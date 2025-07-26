using System.ComponentModel.DataAnnotations;

namespace SimpleEcommerce_BackEnd.Models.Dtos.Address
{
    public class UpdateAddressDto
    {
        [Required(ErrorMessage = "Line 1 is required.")]
        [StringLength(255, ErrorMessage = "Line 1 cannot exceed 255 characters.")]
        public required string Line1 { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
        public required string City { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters.")]
        public required string Country { get; set; }
    }
}