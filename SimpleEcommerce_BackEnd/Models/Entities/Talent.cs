using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleEcommerce.Models.Entities
{
    public class Talent
    {
        public Guid TalentID { get; set; }
        public required string Name { get; set; }
        public required string Generation { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
        //Cú pháp rút gọn: public ICollection<Product> Products { get; set; } = [];
    }
}