using System.ComponentModel.DataAnnotations;

namespace TechTask.Api.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(255, ErrorMessage = "Category name can't be longer than 255 characters.")]
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
