using Microsoft.EntityFrameworkCore;
using TechTask.Api.Database;
using TechTask.Api.Interfaces;
using TechTask.Api.Models;

namespace TechTask.Api.Services
{
    public class ProductsService : IProductsService
    {
        private readonly AppDbContext _dbContext;

        public ProductsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> PostAsync(Product product)
        {
            try
            {
                await _dbContext.AddAsync(product);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Product updProduct)
        {
            var existingEntity = await _dbContext.Products.FindAsync(updProduct.Id);
            if (existingEntity == null) return false; //Nothing to update

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(updProduct);

            //Not using this because its update all the properties even they are not changed
            //Context.Entry(entity).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null) return false;

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
