using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Models.DTO.Review
{
    public class UpdateReviewDto
    {
        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 50, ErrorMessage = "Rating must be between 1 and 50.")]
        [DefaultValue(10)]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        [DefaultValue(1000)]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Customer ID is required.")]
        [DefaultValue(1000)]
        public int CustomerId { get; set; }
    }
}
