using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Models.DTO.Supplier
{
    public class UpdateSupplierDto
    {
        [Required(ErrorMessage = "Supplier name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        [DefaultValue("Default Supplier Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [DefaultValue("supplier@example.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [MaxLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
        [DefaultValue("1234567890")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "IsActive status is required.")]
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}
