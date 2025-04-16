namespace ProductsManagementSystem.Models.DTO.Order
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class OrderCreateDto
    {
        [Required]
        [DefaultValue("2025-01-01")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Total amount is required.")]
        [Range(1.0, 500.00, ErrorMessage = "Total amount must be greater than zero.")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Order status is required.")]
        [MaxLength(20, ErrorMessage = "Order status cannot exceed 20 characters.")]
        [DefaultValue("Pending")]
        public string OrderStatus { get; set; } = "Pending";

        [Required(ErrorMessage = "Payment method is required.")]
        [MaxLength(50, ErrorMessage = "Payment method cannot exceed 50 characters.")]
        [DefaultValue("Credit Card")]
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "Shipping address is required.")]
        [MaxLength(200, ErrorMessage = "Shipping address cannot exceed 200 characters.")]
        [DefaultValue("123 Default Street, City, Country")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Customer ID is required.")]
        [DefaultValue(1000)]
        public int CustomerId { get; set; }
    }
}
