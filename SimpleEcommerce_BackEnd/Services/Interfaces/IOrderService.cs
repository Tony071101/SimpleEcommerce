using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleEcommerce.Models;
using SimpleEcommerce.Models.Entities;

namespace SimpleEcommerce.Services.Interfaces
{
    public interface IOrderService
    {
        // Customer Operations:
        Task<Order> CreateOrderAsync(Guid userId, Order order); // Customer makes an order
        Task<IEnumerable<Order>> GetCustomerOrdersAsync(Guid userId); // Customer views their own orders
        Task<Order?> GetCustomerOrderByIdAsync(Guid userId, Guid orderId); // Customer views specific order
        // Task<bool> CancelOrderAsync(Guid customerId, Guid orderId); // Limited update for customer

        // Admin Operations:
        Task<IEnumerable<Order>> GetAllOrdersAsync(); // Admin views all orders
        Task<Order?> GetOrderByIdAsync(Guid orderId); // Admin views any specific order
        Task<Order?> UpdateOrderStatusAsync(Guid orderId, string newStatus); // Admin updates order status
    }
}