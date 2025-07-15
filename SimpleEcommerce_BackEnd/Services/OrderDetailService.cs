using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Data;
using SimpleEcommerce.Models.Entities;
using SimpleEcommerce.Services.Interfaces;

namespace SimpleEcommerce.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly ApplicationDbContext dbContext;
        public OrderDetailService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<OrderDetail?> GetOrderDetailByIdAsync(Guid id) => await dbContext.OrderDetails.FindAsync(id);

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(Guid orderId)
        {
            return await dbContext.OrderDetails
                                   .Where(od => od.OrderID == orderId)
                                   .Include(od => od.Product)
                                   .ToListAsync();
        }
    }
}