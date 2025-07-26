using SimpleEcommerce_BackEnd.Models.Entities;

namespace SimpleEcommerce_BackEnd.Services.Interfaces
{
    public interface IOrderDetailService
    {
        Task<OrderDetail?> GetOrderDetailByIdAsync(Guid id);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(Guid orderId);
    }
}