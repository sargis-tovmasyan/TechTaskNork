using TechTask.Api.Models;

namespace TechTask.Api.Interfaces
{
    public interface ICategoriesService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<bool> PostAsync(Category category);
        Task<bool> UpdateAsync(Category updCategory);
        Task<bool> DeleteByIdAsync(int id);
    }
}
