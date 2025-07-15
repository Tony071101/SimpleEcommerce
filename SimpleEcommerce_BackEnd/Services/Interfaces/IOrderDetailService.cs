using SimpleEcommerce.Models.Entities;

namespace SimpleEcommerce.Services.Interfaces
{
    public interface IOrderDetailService
    {
        Task<OrderDetail?> GetOrderDetailByIdAsync(Guid id);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(Guid orderId);
    }
}