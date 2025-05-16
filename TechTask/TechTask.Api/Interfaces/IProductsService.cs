using TechTask.Api.Models;

namespace TechTask.Api.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<bool> PostProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product updProduct);
        Task<bool> DeleteProductByIdAsync(int id);
    }
}
