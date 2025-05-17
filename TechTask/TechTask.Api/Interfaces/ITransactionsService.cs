using TechTask.Api.Models;

namespace TechTask.Api.Interfaces
{
    public interface ITransactionsService
    {
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(int id);
        Task<bool> PostAsync(Transaction transaction);
        Task<bool> UpdateAsync(Transaction updTransaction);
        Task<bool> DeleteByIdAsync(int id);
    }
}
