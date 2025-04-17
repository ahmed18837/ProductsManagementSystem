using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Models.DTO.Supplier
{
    public class SupplierFilterDto
    {
        public string? Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? CompanyName { get; set; }

        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }

        public bool? IsActive { get; set; }
    }
}