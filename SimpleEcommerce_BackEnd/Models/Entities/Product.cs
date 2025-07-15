using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleEcommerce.Models.Entities
{
    public class Product
    {
        public Guid ProductID { get; set; }
        public required string ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductStock { get; set; }
        public string? ProductImageUrl { get; set; }

        //FK
        public required Guid CategoryID { get; set; }
        public Category Category { get; set; } = null!;

        public required Guid TalentID { get; set; }
        public Talent Talent { get; set; } = null!;

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        //Cú pháp rút gọn: public ICollection<OrderDetail> OrderDetails { get; set; } = [];
    }
}