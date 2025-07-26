using Microsoft.EntityFrameworkCore;
using SimpleEcommerce_BackEnd.Data;
using SimpleEcommerce_BackEnd.Models.Entities;
using SimpleEcommerce_BackEnd.Services.Interfaces;

namespace SimpleEcommerce_BackEnd.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext dbContext;

        public CartService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Cart>> GetCartByCustomerIdAsync(Guid userId)
        {
            return await dbContext.Carts
                                   .Where(c => c.UserID == userId)
                                   .Include(c => c.Product)
                                   .ToListAsync();
        }

        public async Task<Cart?> GetCartItemByCustomerAndProductAsync(Guid userId, Guid productId)
        {
            return await dbContext.Carts
                                   .FirstOrDefaultAsync(c => c.UserID == userId && c.ProductID == productId);
        }

        public async Task<Cart?> AddOrUpdateCartItemAsync(Guid userId, Guid productId, int quantity)
        {
            var existingCartItem = await GetCartItemByCustomerAndProductAsync(userId, productId);

            if (quantity <= 0) // Nếu số lượng mới là 0 hoặc âm, hãy xóa mặt hàng
            {
                if (existingCartItem != null)
                {
                    dbContext.Carts.Remove(existingCartItem);
                    await dbContext.SaveChangesAsync();
                }
                return null; // Trả về null khi xóa hoặc không tìm thấy để cập nhật với số lượng 0
            }

            if (existingCartItem == null)
            {
                // Item not in cart, add new
                var newCartItem = new Cart
                {
                    CartID = Guid.NewGuid(),
                    UserID = userId,
                    ProductID = productId,
                    Quantity = quantity // Đặt số lượng mới
                };
                await dbContext.Carts.AddAsync(newCartItem);
                await dbContext.SaveChangesAsync();
                return newCartItem;
            }
            else
            {
                // Item already in cart, update quantity
                existingCartItem.Quantity = quantity; // Đặt số lượng MỚI
                await dbContext.SaveChangesAsync();
                return existingCartItem;
            }
        }

        public async Task<bool> RemoveCartItemAsync(Guid userId, Guid productId)
        {
            var cartItemToDelete = await GetCartItemByCustomerAndProductAsync(userId, productId);

            if (cartItemToDelete == null)
            {
                return false; // Item not found in cart
            }

            dbContext.Carts.Remove(cartItemToDelete);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCartAsync(Guid userId)
        {
            var cartItemsToClear = await dbContext.Carts
                                                   .Where(c => c.UserID == userId)
                                                   .ToListAsync();
            if (cartItemsToClear.Count == 0)
            {
                return false; // Cart is already empty
            }

            dbContext.Carts.RemoveRange(cartItemsToClear);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}