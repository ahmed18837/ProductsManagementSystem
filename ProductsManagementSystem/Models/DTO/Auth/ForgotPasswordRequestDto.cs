using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Models.DTO.Auth
{
    public class ForgotPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? ClientUrl { get; set; }
    }
}
