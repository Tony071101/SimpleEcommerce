using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleEcommerce.Models.Entities
{
    public class Category
    {
        public Guid CategoryID { get; set; }
        public required string CategoryName { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        //Cú pháp rút gọn: public ICollection<Product> Products { get; set; } = [];
    }
}