namespace ProductsManagementSystem.Models.DTO.Customer
{
    public class CustomerFilterDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DobStart { get; set; }
        public DateTime? DobEnd { get; set; }
        public DateTime? RegStart { get; set; }
        public DateTime? RegEnd { get; set; }
    }
}
