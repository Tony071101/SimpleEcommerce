using Microsoft.EntityFrameworkCore;
using SimpleEcommerce_BackEnd.Data;
using SimpleEcommerce_BackEnd.Models.Entities;
using SimpleEcommerce_BackEnd.Services.Interfaces;

namespace SimpleEcommerce_BackEnd.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext dbContext;

        public ProductService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetAllProductSync() => await dbContext.Products
                                                                            .Include(p => p.Category)
                                                                            .Include(p => p.Talent)
                                                                            .ToListAsync();

        public async Task<Product?> GetProductByIdSync(Guid id) => await dbContext.Products
                                                                            .Include(p => p.Category)
                                                                            .Include(p => p.Talent)
                                                                            .FirstOrDefaultAsync(p => p.ProductID == id);

        public async Task<Product> AddProductSync(Product product)
        {
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateProductSync(Guid id, Product product)
        {
            var existingProduct = await dbContext.Products.FindAsync(id);
            if (existingProduct == null) return null;

            // Cập nhật các thuộc tính từ entity 'product' được truyền vào
            existingProduct.ProductName = product.ProductName;
            existingProduct.ProductDescription = product.ProductDescription;
            existingProduct.ProductPrice = product.ProductPrice;
            existingProduct.ProductStock = product.ProductStock;
            existingProduct.ProductImageUrl = product.ProductImageUrl;
            existingProduct.CategoryID = product.CategoryID;
            existingProduct.TalentID = product.TalentID;

            await dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProductSync(Guid id)
        {
            var product = await dbContext.Products.FindAsync(id);
            if (product == null) return false;

            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}