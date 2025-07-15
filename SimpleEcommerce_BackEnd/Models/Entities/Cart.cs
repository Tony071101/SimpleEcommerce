using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleEcommerce.Models.Entities
{
    public class Cart
    {
        public Guid CartID { get; set; }

        public Guid UserID { get; set; }
        public User User { get; set; } = null!;

        public Guid ProductID { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
    }
}