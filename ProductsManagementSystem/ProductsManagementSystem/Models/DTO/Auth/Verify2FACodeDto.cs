using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Models.DTO.Auth
{
    public class Verify2FACodeDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [DefaultValue("user@example.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Code is required.")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Reset Code must be 8 characters.")]
        [DefaultValue("12345678")]
        public string Code { get; set; }
    }
}
