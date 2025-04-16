using ProductsManagementSystem.Models.DTO.Customer;
using ProductsManagementSystem.Models.Entities;
using ProductsManagementSystem.Repository.Generic;

namespace ProductsManagementSystem.Repository.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<IEnumerable<Customer>> GetCustomersBornAfterYearAsync(int year);
        Task<IEnumerable<Customer>> GetFilteredCustomersAsync(CustomerFilterDto filterDto);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> PhoneExistsAsync(string phoneNumber);
    }
}

