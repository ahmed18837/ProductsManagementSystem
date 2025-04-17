using ProductsManagementSystem.Models.DTO.Category;
using ProductsManagementSystem.Models.Entities;
using ProductsManagementSystem.Repository.Generic;

namespace ProductsManagementSystem.Repository.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category> GetByNameAsync(string name);
        Task<IEnumerable<Category>> GetCategoriesAddedInLastDay(int days);
        Task<IEnumerable<CategoryProductCountDto>> GetCategoryProductCountsAsync();
        Task<IEnumerable<Category>> GetFilteredCategoriesAsync(string? name, bool? isActive, DateTime? startDate, DateTime? endDate);
    }
}
