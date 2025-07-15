using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Data;
using SimpleEcommerce.Models;
using SimpleEcommerce.Models.Entities;
using SimpleEcommerce.Services.Interfaces;

namespace SimpleEcommerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IProductService productService;
        public OrderService(ApplicationDbContext dbContext, IProductService productService)
        {
            this.dbContext = dbContext;
            this.productService = productService;
        }

        #region Admin
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await dbContext.Orders
                                   .Include(o => o.User) // Eager load customer info
                                   .Include(o => o.OrderDetails)
                                       .ThenInclude(od => od.Product)
                                   .OrderByDescending(o => o.OrderDate)
                                   .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            return await dbContext.Orders
                                   .Include(o => o.User)
                                   .Include(o => o.OrderDetails)
                                       .ThenInclude(od => od.Product)
                                   .FirstOrDefaultAsync(o => o.OrderID == orderId);
        }

        public async Task<Order?> UpdateOrderStatusAsync(Guid orderId, string newStatus)
        {
            var orderToUpdate = await dbContext.Orders.FindAsync(orderId);
            if (orderToUpdate == null)
            {
                return null;
            }

            // Basic status validation
            var allowedStatuses = new List<string> { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };
            if (!allowedStatuses.Contains(newStatus))
            {
                throw new ArgumentException("Invalid order status provided.");
            }

            orderToUpdate.Status = newStatus;
            await dbContext.SaveChangesAsync();
            return orderToUpdate;
        }

        // public async Task<bool> CancelOrderAsync(Guid customerId, Guid orderId)
        // {
        //     var order = await _dbContext.Orders.Where(o => o.CustomerID == customerId && o.OrderID == orderId).FirstOrDefaultAsync();
        //     if (order == null || order.Status != "Processing") // Only allow cancellation if order is processing
        //     {
        //         return false;
        //     }
        //     order.Status = "Cancelled";
        //     await _dbContext.SaveChangesAsync();
        //     return true;
        // }
        #endregion

        #region Customer
        public async Task<Order> CreateOrderAsync(Guid userId, Order order)
        {
            order.OrderID = Guid.NewGuid();
            order.UserID = userId; // Ensure customer ID is set/verified by token
            order.OrderDate = DateTime.Now;
            order.Status = "Pending";

            decimal totalAmount = 0;

            foreach (var orderDetail in order.OrderDetails) // Lặp qua OrderDetail Entities
            {
                // Vẫn phải FETCH THE REAL PRODUCT PRICE AND STOCK FROM THE DATABASE
                // Bất kể việc ánh xạ DTO -> Entity đã diễn ra
                var product = await productService.GetProductByIdSync(orderDetail.ProductID);
                if (product == null)
                {
                    throw new InvalidOperationException($"Product with ID {orderDetail.ProductID} not found.");
                }

                if (product.ProductStock < orderDetail.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for product {product.ProductName}. Available: {product.ProductStock}, Requested: {orderDetail.Quantity}");
                }

                // Cập nhật giá và giảm số lượng tồn kho
                orderDetail.Price = product.ProductPrice; // Ghi đè giá từ DB
                product.ProductStock -= orderDetail.Quantity; // Giảm tồn kho

                // Gán OrderID cho OrderDetail
                orderDetail.OrderID = order.OrderID;
                orderDetail.OrderDetailID = Guid.NewGuid(); // Gán ID cho OrderDetail
                
                totalAmount += orderDetail.Quantity * orderDetail.Price;
            }

            order.TotalAmount = totalAmount;

            await dbContext.Orders.AddAsync(order);

            // Optional: Clear customer's cart after successful order creation
            var cartItems = await dbContext.Carts.Where(c => c.UserID == userId).ToListAsync();
            dbContext.Carts.RemoveRange(cartItems);

            await dbContext.SaveChangesAsync();
            return order;
        }
        
        public async Task<IEnumerable<Order>> GetCustomerOrdersAsync(Guid userId)
        {
            return await dbContext.Orders
                                   .Where(o => o.UserID == userId)
                                   .Include(o => o.OrderDetails)
                                       .ThenInclude(od => od.Product) // Eager load product details for each order item
                                   .OrderByDescending(o => o.OrderDate)
                                   .ToListAsync();
        }
        
        public async Task<Order?> GetCustomerOrderByIdAsync(Guid userId, Guid orderId)
        {
            return await dbContext.Orders
                                   .Where(o => o.UserID == userId && o.OrderID == orderId)
                                   .Include(o => o.OrderDetails)
                                       .ThenInclude(od => od.Product)
                                   .FirstOrDefaultAsync();
        }
        #endregion
    }
}