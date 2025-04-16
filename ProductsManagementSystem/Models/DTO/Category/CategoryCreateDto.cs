using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Models.DTO.Category
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        [DefaultValue("Name")]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Created Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [DefaultValue("2025-02-15")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}
