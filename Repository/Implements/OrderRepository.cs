using Microsoft.EntityFrameworkCore;
using ProductsManagementSystem.Data;
using ProductsManagementSystem.Models.DTO.Order;
using ProductsManagementSystem.Models.Entities;
using ProductsManagementSystem.Repository.Generic;
using ProductsManagementSystem.Repository.Interfaces;

namespace ProductsManagementSystem.Repository.Implements
{
    public class OrderRepository(AppDbContext dbContext) : GenericRepository<Order>(dbContext), IOrderRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<IEnumerable<OrderDto>> GetOrdersAddedInLastDay(int days)
        {
            var fromTime = DateTime.Now.AddDays(-days);
            return await _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .Where(o => o.OrderDate >= fromTime)
                .Select(order => new OrderDto
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    OrderStatus = order.OrderStatus,
                    PaymentMethod = order.PaymentMethod,
                    ShippingAddress = order.ShippingAddress,
                    CustomerId = order.CustomerId,
                    CustomerName = order.Customer != null ? order.Customer.FirstName + " " + order.Customer.LastName : "N/A"
                })
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            return await _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .Select(order => new OrderDto
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    OrderStatus = order.OrderStatus,
                    PaymentMethod = order.PaymentMethod,
                    ShippingAddress = order.ShippingAddress,
                    CustomerId = order.CustomerId,
                    CustomerName = order.Customer != null ? order.Customer.FirstName + " " + order.Customer.LastName : "N/A"
                })
                .ToListAsync();
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            return await _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .Where(o => o.Id == id)
                .Select(order => new OrderDto
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    OrderStatus = order.OrderStatus,
                    PaymentMethod = order.PaymentMethod,
                    ShippingAddress = order.ShippingAddress,
                    CustomerId = order.CustomerId,
                    CustomerName = order.Customer != null ? order.Customer.FirstName + " " + order.Customer.LastName : "N/A"
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<OrderDto>> GetFilteredOrdersAsync(OrderFilterDto filterDto)
        {
            var query = _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .AsQueryable();

            if (filterDto.OrderDateFrom.HasValue)
                query = query.Where(o => o.OrderDate >= filterDto.OrderDateFrom.Value);

            if (filterDto.OrderDateTo.HasValue)
                query = query.Where(o => o.OrderDate <= filterDto.OrderDateTo.Value);

            if (filterDto.MinTotalAmount.HasValue)
                query = query.Where(o => o.TotalAmount >= filterDto.MinTotalAmount.Value);

            if (filterDto.MaxTotalAmount.HasValue)
                query = query.Where(o => o.TotalAmount <= filterDto.MaxTotalAmount.Value);

            if (!string.IsNullOrWhiteSpace(filterDto.OrderStatus))
                query = query.Where(o => o.OrderStatus == filterDto.OrderStatus);

            if (!string.IsNullOrWhiteSpace(filterDto.PaymentMethod))
                query = query.Where(o => o.PaymentMethod == filterDto.PaymentMethod);

            if (!string.IsNullOrWhiteSpace(filterDto.ShippingAddress))
                query = query.Where(o => o.ShippingAddress.Contains(filterDto.ShippingAddress));

            if (filterDto.CustomerId.HasValue)
                query = query.Where(o => o.CustomerId == filterDto.CustomerId.Value);

            query = filterDto.SortBy?.ToLower() switch
            {
                "date" => filterDto.IsAscending ? query.OrderBy(o => o.OrderDate) : query.OrderByDescending(o => o.OrderDate),
                "amount" => filterDto.IsAscending ? query.OrderBy(o => o.TotalAmount) : query.OrderByDescending(o => o.TotalAmount),
                _ => query
            };

            return await query
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    OrderStatus = o.OrderStatus,
                    PaymentMethod = o.PaymentMethod,
                    ShippingAddress = o.ShippingAddress,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer != null ? o.Customer.FirstName + " " + o.Customer.LastName : "N/A"
                })
                .ToListAsync();
        }

        public async Task<bool> CustomerExistsAsync(int customerId)
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .AnyAsync(c => c.Id == customerId);
        }
    }
}
