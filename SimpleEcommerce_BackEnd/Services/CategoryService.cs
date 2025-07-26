using Microsoft.EntityFrameworkCore;
using SimpleEcommerce_BackEnd.Data;
using SimpleEcommerce_BackEnd.Models.Entities;
using SimpleEcommerce_BackEnd.Services.Interfaces;

namespace SimpleEcommerce_BackEnd.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext dbContext;
        public CategoryService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            return await dbContext.Categories.FindAsync(id);
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            // Đảm bảo CategoryID được sinh ra nếu nó là Guid.Empty
            if (category.CategoryID == Guid.Empty)
            {
                category.CategoryID = Guid.NewGuid();
            }
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateCategoryAsync(Guid id, Category category)
        {
            var existingCategory = await dbContext.Categories.FindAsync(id);
            if (existingCategory == null)
            {
                return null; // Category not found
            }

            existingCategory.CategoryName = category.CategoryName;

            await dbContext.SaveChangesAsync();
            return existingCategory;
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var categoryToDelete = await dbContext.Categories.FindAsync(id);
            if (categoryToDelete == null)
            {
                return false; // Category not found
            }

            dbContext.Categories.Remove(categoryToDelete);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}