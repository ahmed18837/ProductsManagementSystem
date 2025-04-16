using ProductsManagementSystem.Models.DTO.Order;
using ProductsManagementSystem.Models.Entities;
using ProductsManagementSystem.Repository.Generic;
namespace ProductsManagementSystem.Repository.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderDto>> GetOrdersAddedInLastDay(int days);

        Task<IEnumerable<OrderDto>> GetFilteredOrdersAsync(OrderFilterDto filterDto);
        Task<bool> CustomerExistsAsync(int customerId);
    }
}
