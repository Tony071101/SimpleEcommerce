using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleEcommerce.Models.Entities
{
    public class OrderDetail
    {
        public Guid OrderDetailID { get; set; }

        public Guid OrderID { get; set; }
        public Order Order { get; set; } = null!;

        public Guid ProductID { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal Price { get; set; } // Giá tại thời điểm mua
    }
}