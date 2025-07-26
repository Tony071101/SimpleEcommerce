using Microsoft.EntityFrameworkCore;
using SimpleEcommerce_BackEnd.Data;
using SimpleEcommerce_BackEnd.Models.Entities;
using SimpleEcommerce_BackEnd.Services.Interfaces;

namespace SimpleEcommerce_BackEnd.Services
{
    public class TalentService : ITalentService
    {
        private readonly ApplicationDbContext dbContext;
        public TalentService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Talent>> GetAllTalentsAsync()
        {
            return await dbContext.Talents.ToListAsync();
        }

        public async Task<Talent?> GetTalentByIdAsync(Guid id)
        {
            return await dbContext.Talents.FindAsync(id);
        }

        public async Task<Talent> AddTalentAsync(Talent talent)
        {
            if (talent.TalentID == Guid.Empty)
            {
                talent.TalentID = Guid.NewGuid();
            }
            await dbContext.Talents.AddAsync(talent);
            await dbContext.SaveChangesAsync();
            return talent;
        }

        public async Task<Talent?> UpdateTalentAsync(Guid id, Talent talent)
        {
            var existingTalent = await dbContext.Talents.FindAsync(id);
            if (existingTalent == null)
            {
                return null; // Category not found
            }

            existingTalent.TalentName = talent.TalentName;
            existingTalent.TalentGeneration = talent.TalentGeneration;

            await dbContext.SaveChangesAsync();
            return existingTalent;
        }

        public async Task<bool> DeleteTalentAsync(Guid id)
        {
            var talentToDelete = await dbContext.Talents.FindAsync(id);
            if (talentToDelete == null)
            {
                return false; // Category not found
            }

            dbContext.Talents.Remove(talentToDelete);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}