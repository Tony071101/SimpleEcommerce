using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleEcommerce.Models.Dtos.Cart
{
    public class CartResponseDto
    {
        public Guid CustomerID { get; set; }
        public List<CartItemResponseDto> Items { get; set; } = new(); // Danh sách các mặt hàng trong giỏ
        public decimal TotalAmount { get; set; } // Tổng giá trị của tất cả các mặt hàng trong giỏ hàng
    }
}