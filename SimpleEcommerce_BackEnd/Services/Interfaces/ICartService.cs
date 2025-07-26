using SimpleEcommerce_BackEnd.Models.Entities;

namespace SimpleEcommerce_BackEnd.Services.Interfaces
{
    public interface ICartService
    {
        // Customer Operations:
        Task<Cart?> GetCartItemByCustomerAndProductAsync(Guid userId, Guid productId);
        Task<IEnumerable<Cart>> GetCartByCustomerIdAsync(Guid userId);
        Task<Cart?> AddOrUpdateCartItemAsync(Guid userId, Guid productId, int quantity); 
        Task<bool> RemoveCartItemAsync(Guid userId, Guid productId);
        Task<bool> ClearCartAsync(Guid userId);
    }
}