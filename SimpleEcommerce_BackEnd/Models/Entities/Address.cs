
namespace SimpleEcommerce.Models.Entities
{
    public class Address
    {
        public Guid AddressID { get; set; }
        public required string Line1 { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }

        public Guid UserID { get; set; }
        public User User { get; set; } = null!;
    }
}