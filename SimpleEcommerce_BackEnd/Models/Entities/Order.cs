using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleEcommerce_BackEnd.Models.Entities
{
    public class Order
    {
        public Guid OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        
        [Column(TypeName = "decimal(18,2)")] // Đảm bảo kiểu dữ liệu trong DB
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";

        public Guid UserID { get; set; }
        public User User { get; set; } = null!;

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        //Cú pháp rút gọn: ICollection<OrderDetail> OrderDetails { get; set; } = [];
    }
}