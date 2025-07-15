using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleEcommerce.Models.Dtos.Cart
{
    public class CartItemResponseDto
    {
        public Guid ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImageUrl { get; set; } // Có thể null nếu sản phẩm không có ảnh
        public decimal UnitPrice { get; set; } // Giá của sản phẩm tại thời điểm hiện tại
        public int Quantity { get; set; }
        public decimal SubTotal => UnitPrice * Quantity; // Thuộc tính tính toán
    }
}