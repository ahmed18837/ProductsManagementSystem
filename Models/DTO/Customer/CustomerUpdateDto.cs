using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Models.DTO.Customer
{
    public class CustomerUpdateDto
    {
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [DefaultValue("FName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [DefaultValue("LName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(50, ErrorMessage = "Email cannot exceed 50 characters.")]
        [DefaultValue("example@email.com")]
        public string Email { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
