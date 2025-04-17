using ProductsManagementSystem.Models.DTO.Supplier;
using ProductsManagementSystem.Models.Entities;
using ProductsManagementSystem.Repository.Generic;

namespace ProductsManagementSystem.Repository.Interfaces
{
    public interface ISupplierRepository : IGenericRepository<Supplier>
    {
        Task<IEnumerable<Supplier>> GetSuppliersAddedInLastDay(int days);
        Task<IEnumerable<Supplier>> GetFilteredSuppliersAsync(SupplierFilterDto filterDto);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> PhoneExistsAsync(string phoneNumber);
    }
}
