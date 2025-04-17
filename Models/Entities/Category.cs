using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsManagementSystem.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "DATE")]
        public DateTime CreatedDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        // Navigation Properties

        public ICollection<Product> Products { get; set; }
    }
}
