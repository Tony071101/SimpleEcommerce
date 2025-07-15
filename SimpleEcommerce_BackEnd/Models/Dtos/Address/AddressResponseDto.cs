

namespace SimpleEcommerce.Models.Dtos.Address
{
    public class AddressResponseDto
    {
        public Guid AddressID { get; set; } // ID của địa chỉ
        public Guid CustomerID { get; set; } // ID của khách hàng sở hữu địa chỉ
        public required string Line1 { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
    }
}