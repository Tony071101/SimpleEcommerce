using SimpleEcommerce_BackEnd.Models.Entities;

namespace SimpleEcommerce_BackEnd.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductSync();
        Task<Product?> GetProductByIdSync(Guid id);
        Task<Product> AddProductSync(Product product);
        Task<Product?> UpdateProductSync(Guid id, Product product);
        Task<bool> DeleteProductSync(Guid id);
    }
}