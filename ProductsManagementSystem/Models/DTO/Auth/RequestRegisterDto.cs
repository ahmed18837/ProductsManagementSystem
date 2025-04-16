using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Models.DTO.Auth
{
    public class RequestRegisterDto
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [MaxLength(50, ErrorMessage = "Full Name cannot exceed 50 characters.")]
        [DefaultValue("FullName")]
        public string FullName { get; set; }

        //[Required(ErrorMessage = "National ID is required.")]
        //[StringLength(14, MinimumLength = 14, ErrorMessage = "National ID must be exactly 14 characters.")]
        //[RegularExpression("^[0-9]{14}$", ErrorMessage = "National ID must contain only numbers.")]
        //[DefaultValue("12345678901234")]
        //public string NationalId { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [DefaultValue("user@example.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [DefaultValue("01234567890")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [MaxLength(64, ErrorMessage = "Password cannot exceed 64 characters.")]
        [DefaultValue("SecurePass123")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [MaxLength(64, ErrorMessage = "Password cannot exceed 64 characters.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DefaultValue("SecurePass123")]
        public string ConfirmPassword { get; set; }
    }
}
