using Microsoft.EntityFrameworkCore;
using TechTask.Api.Database;
using TechTask.Api.Interfaces;
using TechTask.Api.Models;

namespace TechTask.Api.Services
{
    public class SuppliersService : ISuppliersService
    {
        private readonly AppDbContext _dbContext;

        public SuppliersService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _dbContext.Suppliers
                .AsNoTracking()
                .Include(s => s.Products)
                .ToListAsync();
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _dbContext.Suppliers
                .AsNoTracking()
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> PostAsync(Supplier supplier)
        {
            try
            {
                await _dbContext.AddAsync(supplier);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Supplier updSupplier)
        {
            var existingEntity = await _dbContext.Suppliers.FindAsync(updSupplier.Id);
            if (existingEntity == null) return false; //Nothing to update

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(updSupplier);

            //Not using this because its update all the properties even they are not changed
            //Context.Entry(entity).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var supplier = await _dbContext.Suppliers.FindAsync(id);

            if (supplier == null) return false;

            _dbContext.Suppliers.Remove(supplier);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
