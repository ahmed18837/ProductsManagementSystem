using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Models.DTO.Review
{
    public class CreateReviewDto
    {
        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 50, ErrorMessage = "Rating must be between 1 and 50.")]
        [DefaultValue(10)]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Comment is required.")]
        [MaxLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
        [DefaultValue("No comment provided.")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Review date is required.")]
        [DefaultValue("2025-01-01")]
        public DateTime ReviewDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Product ID is required.")]
        [DefaultValue(1000)]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Customer ID is required.")]
        [DefaultValue(1000)]
        public int CustomerId { get; set; }
    }

}
