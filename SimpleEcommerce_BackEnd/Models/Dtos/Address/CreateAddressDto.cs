using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleEcommerce.Models.Dtos.Address
{
    public class CreateAddressDto
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

        // KHÔNG BAO GỒM CustomerID ở đây. Nó sẽ được lấy từ JWT/Claim của người dùng hiện tại.
    }
}