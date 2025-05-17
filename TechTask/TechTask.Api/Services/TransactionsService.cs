using Microsoft.EntityFrameworkCore;
using TechTask.Api.Database;
using TechTask.Api.Interfaces;
using TechTask.Api.Models;

namespace TechTask.Api.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly AppDbContext _dbContext;

        public TransactionsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _dbContext.Transactions
                .AsNoTracking()
                .Include(t => t.Product)
                .ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _dbContext.Transactions
                .AsNoTracking()
                .Include(t => t.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> PostAsync(Transaction transaction)
        {
            var product = await _dbContext.Products.FindAsync(transaction.ProductId);
            if (product == null) return false;

            switch (transaction.Type)
            {
                case TransactionType.Sale:
                    if (product.StockQuantity < transaction.Quantity) return false;

                    product.StockQuantity -= transaction.Quantity;
                    break;
                case TransactionType.Purchase:
                    product.StockQuantity += transaction.Quantity;
                    break;
                default:
                    return false;
            }

            try
            {
                await _dbContext.AddAsync(transaction);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Transaction updTransaction)
        {
            var existingEntity = await _dbContext.Transactions.FindAsync(updTransaction.Id);
            if (existingEntity == null) return false; //Nothing to update

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(updTransaction);

            //Not using this because its update all the properties even they are not changed
            //Context.Entry(entity).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var transaction = await _dbContext.Transactions.FindAsync(id);

            if (transaction == null) return false;

            _dbContext.Transactions.Remove(transaction);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
