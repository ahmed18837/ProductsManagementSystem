using AutoMapper;
using ProductsManagementSystem.Models.DTO.Order;
using ProductsManagementSystem.Models.Entities;
using ProductsManagementSystem.Repository.Interfaces;
using ProductsManagementSystem.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Services.Implements
{
    public class OrderService(IOrderRepository orderRepository, IMapper mapper) : IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();

            if (orders == null || !orders.Any())
            {
                throw new KeyNotFoundException("Not Orders Founded!");
            }
            return orders;
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("Id must be greater than zero.");
            }
            var order = await _orderRepository.GetOrderByIdAsync(id) ??
                throw new KeyNotFoundException("Order not found");
            return order;
        }

        public async Task AddAsync(OrderCreateDto orderCreateDto)
        {
            if (orderCreateDto == null)
                throw new ValidationException("Input data cannot be null");

            if (orderCreateDto.OrderDate < DateTime.Now.Date)
            {
                throw new ValidationException("Order date cannot be in the past.");
            }
            if (!await _orderRepository.CustomerExistsAsync(orderCreateDto.CustomerId))
            {
                throw new ValidationException($"Customer does not exist.");
            }

            var order = _mapper.Map<Order>(orderCreateDto);

            await _orderRepository.AddAsync(order);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("ID must be greater than zero.");
            }
            var order = await _orderRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException($"Order with {id} not found");

            await _orderRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAddedInLastDay(int days)
        {
            if (days < 0) throw new Exception("Invalid Value");

            var orders = await _orderRepository.GetOrdersAddedInLastDay(days) ??
                throw new Exception("No Orders Found!");

            var ordersDto = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return ordersDto;
        }

        public async Task UpdateAsync(int id, OrderUpdateDto orderUpdateDto)
        {
            if (id <= 0)
            {
                throw new ValidationException("Id must be greater than zero.");
            }
            var order = await _orderRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Order not found");

            if (!await _orderRepository.CustomerExistsAsync(orderUpdateDto.CustomerId))
            {
                throw new ValidationException($"Customer does not exist.");
            }
            // Check if all input values are the same as the current values
            if (order.TotalAmount == orderUpdateDto.TotalAmount
                && order.OrderStatus == orderUpdateDto.OrderStatus && order.PaymentMethod == orderUpdateDto.PaymentMethod
                && order.ShippingAddress == orderUpdateDto.ShippingAddress && order.CustomerId == orderUpdateDto.CustomerId)
            {
                throw new Exception("No changes detected, order remains unchanged");
            }

            _mapper.Map(orderUpdateDto, order);

            await _orderRepository.UpdateAsync(id, order);
        }

        public async Task<IEnumerable<OrderDto>> GetFilteredOrdersAsync(OrderFilterDto filterDto)
        {
            var orders = await _orderRepository.GetFilteredOrdersAsync(filterDto);
            if (orders == null || !orders.Any())
                throw new Exception("There no Orders");

            return orders;
        }
    }
}
