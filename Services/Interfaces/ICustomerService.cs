using ProductsManagementSystem.Models.DTO.Customer;

namespace ProductsManagementSystem.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllAsync();
        Task<CustomerDto> GetByIdAsync(int id);
        Task AddAsync(CustomerCreateDto entity);
        Task UpdateAsync(int id, CustomerUpdateDto entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<CustomerDto>> GetCustomersBornAfterYearAsync(int year);
        Task<IEnumerable<CustomerDto>> GetFilteredCustomersAsync(CustomerFilterDto filterDto);
    }
}

