using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Models.DTO.Order
{
    public class OrderFilterDto
    {
        public DateTime? OrderDateFrom { get; set; }
        public DateTime? OrderDateTo { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Min total amount must be greater than zero.")]
        public decimal? MinTotalAmount { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Max total amount must be greater than zero.")]
        public decimal? MaxTotalAmount { get; set; }

        [MaxLength(50, ErrorMessage = "Order status cannot exceed 50 characters.")]
        public string? OrderStatus { get; set; }

        [MaxLength(50, ErrorMessage = "Payment method cannot exceed 50 characters.")]
        public string? PaymentMethod { get; set; }

        [MaxLength(100, ErrorMessage = "Shipping address cannot exceed 100 characters.")]
        public string? ShippingAddress { get; set; }

        public int? CustomerId { get; set; }

        public string? SortBy { get; set; }
        public bool IsAscending { get; set; }
    }
}