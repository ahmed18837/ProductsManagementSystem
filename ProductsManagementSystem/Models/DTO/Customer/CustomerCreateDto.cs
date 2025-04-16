using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsManagementSystem.Models.DTO.Customer
{
    public class CustomerCreateDto
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

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^01\d{9}$", ErrorMessage = "Phone number must start with '01' and be exactly 11 digits.")]
        [MaxLength(11, ErrorMessage = "Phone number must be exactly 11 digits.")]
        [MinLength(11, ErrorMessage = "Phone number must be exactly 11 digits.")]
        [DefaultValue("01012345678")]
        public string PhoneNumber { get; set; }

        [MaxLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
        [DefaultValue("123 Default Street, City")]
        public string Address { get; set; }

        [MaxLength(20, ErrorMessage = "Gender cannot exceed 20 characters.")]
        [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be 'Male' or 'Female'.")]
        [DefaultValue("Male")]
        public string Gender { get; set; }

        [Column(TypeName = "DATE")]
        [DefaultValue("2000-01-01")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [Required]
        [Column(TypeName = "DATE")]
        [DefaultValue("2025-01-01")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
