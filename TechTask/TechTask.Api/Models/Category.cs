using System.ComponentModel.DataAnnotations;

namespace TechTask.Api.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(255, ErrorMessage = "Product name cannot exceed 255 characters.")]
        public string Name { get; set; }

        public List<Product>? Products { get; set; }
    }
}
