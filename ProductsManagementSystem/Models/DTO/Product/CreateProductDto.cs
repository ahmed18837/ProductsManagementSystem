namespace ProductsManagementSystem.Models.DTO.Product
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class CreateProductDto
    {
        [Required(ErrorMessage = "Product name is required.")]
        [MaxLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        [DefaultValue("Product1")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [DefaultValue("No description provided.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 100000.00, ErrorMessage = "Price must be between 0.01 and 100,000.")]
        [DefaultValue(100)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required.")]
        [Range(0, 5000, ErrorMessage = "Stock quantity must be a non-negative value.")]
        [DefaultValue(50)]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Created date is required.")]
        [DataType(DataType.Date)]
        [DefaultValue("2025-01-01")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Product availability status is required.")]
        [DefaultValue(true)]
        public bool IsAvailable { get; set; } = true;

        [Required(ErrorMessage = "Product image is required.")]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Category ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Category ID.")]
        [DefaultValue(1000)]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Supplier ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Supplier ID.")]
        [DefaultValue(1000)]
        public int SupplierId { get; set; }
    }
}
