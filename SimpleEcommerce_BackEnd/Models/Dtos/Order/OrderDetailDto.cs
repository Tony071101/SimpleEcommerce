using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleEcommerce.Models.Dtos.Order
{
    public class OrderDetailDto
    {
        public Guid OrderDetailID { get; set; }
        public Guid ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }     // Giá tại thời điểm mua
        public int Quantity { get; set; }
        public decimal SubTotal => Quantity * Price;
    }
}