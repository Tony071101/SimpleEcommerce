namespace SimpleEcommerce_BackEnd.Models.Dtos.Order
{
    public class OrderResponseDto
    {
        public Guid OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;

        public Guid UserID { get; set; } // ID của người dùng đặt hàng
        public string Username { get; set; } = string.Empty; // Tên người dùng đặt hàng

        public List<OrderDetailDto> Items { get; set; } = new();
    }
}