using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsManagementSystem.Models.Entities
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string CompanyName { get; set; }

        [Required]
        [Column(TypeName = "DATE")]
        public DateTime CreatedDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        // Navigation Properties

        public ICollection<Product> Products { get; set; }
    }
}
