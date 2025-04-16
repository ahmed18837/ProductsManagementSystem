using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Models.DTO
{
    public class TokenRequestDto
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
