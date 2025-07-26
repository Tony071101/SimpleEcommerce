using SimpleEcommerce_BackEnd.Models.Entities;

namespace SimpleEcommerce_BackEnd.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(Guid id);
        Task<Category> AddCategoryAsync(Category category);
        Task<Category?> UpdateCategoryAsync(Guid id, Category category);
        Task<bool> DeleteCategoryAsync(Guid id);
    }
}