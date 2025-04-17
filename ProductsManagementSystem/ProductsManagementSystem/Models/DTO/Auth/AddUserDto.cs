using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Models.DTO.Auth
{
    public class AddUserDto : RequestRegisterDto
    {
        [Required(ErrorMessage = "Role Name is required")]
        public string Role { get; set; }
    }
}