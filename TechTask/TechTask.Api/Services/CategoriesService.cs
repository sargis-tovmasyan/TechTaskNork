using Microsoft.EntityFrameworkCore;
using TechTask.Api.Database;
using TechTask.Api.Interfaces;
using TechTask.Api.Models;

namespace TechTask.Api.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly AppDbContext _dbContext;

        public CategoriesService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbContext.Categories
                .AsNoTracking()
                .Include(c => c.Products)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _dbContext.Categories
                .AsNoTracking()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> PostAsync(Category category)
        {
            try
            {
                await _dbContext.AddAsync(category);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Category updCategory)
        {
            var existingEntity = await _dbContext.Categories.FindAsync(updCategory.Id);
            if (existingEntity == null) return false; //Nothing to update

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(updCategory);

            //Not using this because its update all the properties even they are not changed
            //Context.Entry(entity).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);

            if (category == null) return false;

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
