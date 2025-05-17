using TechTask.Api.Models;

namespace TechTask.Api.Interfaces
{
    public interface IProductsService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<bool> PostAsync(Product product);
        Task<bool> UpdateAsync(Product updProduct);
        Task<bool> DeleteByIdAsync(int id);
    }
}
