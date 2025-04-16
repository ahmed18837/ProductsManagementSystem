using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsManagementSystem.Models.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Rating { get; set; }

        [MaxLength(200)]
        public string Comment { get; set; }

        [Required]
        [Column(TypeName = "DATE")]
        public DateTime ReviewDate { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
