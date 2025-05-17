using TechTask.Api.Models;

namespace TechTask.Api.Interfaces
{
    public interface ISuppliersService
    {
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<Supplier?> GetByIdAsync(int id);
        Task<bool> PostAsync(Supplier product);
        Task<bool> UpdateAsync(Supplier updSupplier);
        Task<bool> DeleteByIdAsync(int id);
    }
}
