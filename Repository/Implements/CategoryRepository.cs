using Microsoft.EntityFrameworkCore;
using ProductsManagementSystem.Data;
using ProductsManagementSystem.Models.DTO.Category;
using ProductsManagementSystem.Models.Entities;
using ProductsManagementSystem.Repository.Generic;
using ProductsManagementSystem.Repository.Interfaces;

namespace ProductsManagementSystem.Repository.Implements
{
    public class CategoryRepository(AppDbContext dbContext) : GenericRepository<Category>(dbContext), ICategoryRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<Category> GetByNameAsync(string name)
        {
            return await _dbContext.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<IEnumerable<Category>> GetCategoriesAddedInLastDay(int days)
        {
            var fromDate = DateTime.Now.AddDays(-days);

            return await _dbContext.Categories
                .AsNoTracking()
                .Where(c => c.CreatedDate >= fromDate)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<CategoryProductCountDto>> GetCategoryProductCountsAsync()
        {
            return await _dbContext.Categories
                .AsNoTracking()
                .Include(c => c.Products)
                .Select(c => new CategoryProductCountDto
                {
                    CategoryId = c.Id,
                    CategoryName = c.Name,
                    ProductCount = c.Products.Count
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetFilteredCategoriesAsync(string? name, bool? isActive, DateTime? startDate, DateTime? endDate)
        {
            var query = _dbContext.Categories
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }

            if (isActive.HasValue)
            {
                query = query.Where(c => c.IsActive == isActive.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(c => c.CreatedDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(c => c.CreatedDate <= endDate.Value);
            }

            return await query.ToListAsync();
        }
    }
}
