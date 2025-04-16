using ProductsManagementSystem.Models.DTO.Supplier;

namespace ProductsManagementSystem.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetAllAsync();
        Task<SupplierDto> GetByIdAsync(int id);
        Task AddAsync(CreateSupplierDto entity);
        Task UpdateAsync(int id, UpdateSupplierDto entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<SupplierDto>> GetSuppliersAddedInLastDay(int days);

        Task<IEnumerable<SupplierDto>> GetFilteredSuppliersAsync(SupplierFilterDto filterDto);
    }
}
