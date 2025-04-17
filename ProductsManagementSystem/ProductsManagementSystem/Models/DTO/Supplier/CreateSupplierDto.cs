using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Models.DTO.Supplier
{
    public class CreateSupplierDto
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
        [RegularExpression(@"^01\d{9}$", ErrorMessage = "Phone number must start with '01' and be exactly 11 digits.")]
        [MaxLength(11, ErrorMessage = "Phone number must be exactly 11 digits.")]
        [MinLength(11, ErrorMessage = "Phone number must be exactly 11 digits.")]
        [DefaultValue("01012345678")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        [DefaultValue("123 Supplier Street, City, Country")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Company name is required.")]
        [MaxLength(100, ErrorMessage = "Company name cannot exceed 100 characters.")]
        [DefaultValue("Default Company Name")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Created date is required.")]
        [DefaultValue("2025-01-01")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "IsActive status is required.")]
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}
