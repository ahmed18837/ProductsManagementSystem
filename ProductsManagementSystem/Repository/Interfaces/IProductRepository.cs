using ProductsManagementSystem.Models.DTO.Product;
using ProductsManagementSystem.Models.Entities;
using ProductsManagementSystem.Repository.Generic;

namespace ProductsManagementSystem.Repository.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductAsync(int id);

        Task<IEnumerable<ProductDto>> GetProductsAddedInLastDay(int days);

        Task<IEnumerable<ProductDto>> GetFilteredProductsAsync(ProductFilterDto filterDto);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryIdAsync(int categoryId);

        Task<bool> CategoryExistsAsync(int categoryId);
        Task<bool> SupplierExistsAsync(int supplierId);
    }
}
