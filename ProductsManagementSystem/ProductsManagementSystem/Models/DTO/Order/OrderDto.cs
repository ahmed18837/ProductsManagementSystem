namespace ProductsManagementSystem.Models.DTO.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; } = "Pending";
        public string PaymentMethod { get; set; }
        public string ShippingAddress { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
