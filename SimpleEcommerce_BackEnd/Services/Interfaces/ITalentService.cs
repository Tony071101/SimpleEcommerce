using SimpleEcommerce_BackEnd.Models.Entities;

namespace SimpleEcommerce_BackEnd.Services.Interfaces
{
    public interface ITalentService
    {
        Task<IEnumerable<Talent>> GetAllTalentsAsync();
        Task<Talent?> GetTalentByIdAsync(Guid id);
        Task<Talent> AddTalentAsync(Talent talent);
        Task<Talent?> UpdateTalentAsync(Guid id, Talent talent);
        Task<bool> DeleteTalentAsync(Guid id);
    }
}