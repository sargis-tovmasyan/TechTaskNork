using System.ComponentModel.DataAnnotations;

namespace TechTask.Api.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Supplier name is required.")]
        [StringLength(255, ErrorMessage = "Supplier name cannot exceed 255 characters.")]
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
