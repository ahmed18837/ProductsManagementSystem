using Microsoft.EntityFrameworkCore;
using ProductsManagementSystem.Data;
using ProductsManagementSystem.Enums;
using ProductsManagementSystem.Models.DTO.Product;
using ProductsManagementSystem.Models.Entities;
using ProductsManagementSystem.Repository.Generic;
using ProductsManagementSystem.Repository.Interfaces;

namespace ProductsManagementSystem.Repository.Implements
{
    public class ProductRepository(AppDbContext dbContext) : GenericRepository<Product>(dbContext), IProductRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            return await _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CreatedDate = p.CreatedDate,
                    IsAvailable = p.IsAvailable,
                    ImagePath = p.ImagePath,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    SupplierId = p.SupplierId,
                    SupplierName = p.Supplier.Name
                })
                .ToListAsync();
        }

        public async Task<ProductDto> GetProductAsync(int id)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CreatedDate = p.CreatedDate,
                    IsAvailable = p.IsAvailable,
                    ImagePath = p.ImagePath,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    SupplierId = p.SupplierId,
                    SupplierName = p.Supplier.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAddedInLastDay(int days)
        {
            var fromDate = DateTime.Now.AddDays(-days);

            return await _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Where(p => p.CreatedDate >= fromDate)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CreatedDate = p.CreatedDate,
                    IsAvailable = p.IsAvailable,
                    ImagePath = p.ImagePath,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    SupplierId = p.SupplierId,
                    SupplierName = p.Supplier.Name
                })
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetFilteredProductsAsync(ProductFilterDto filterDto)
        {
            var query = _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .AsQueryable();

            if (filterDto.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filterDto.MinPrice);

            if (filterDto.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filterDto.MaxPrice);

            if (filterDto.IsAvailable.HasValue)
                query = query.Where(p => p.IsAvailable == filterDto.IsAvailable.Value);

            if (filterDto.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == filterDto.CategoryId);

            query = filterDto.OrderBy.ToLower() switch
            {
                "price" => filterDto.OrderDirection == OrderDirection.Ascending
                    ? query.OrderBy(p => p.Price)
                    : query.OrderByDescending(p => p.Price),
                "name" => filterDto.OrderDirection == OrderDirection.Ascending
                    ? query.OrderBy(p => p.Name)
                    : query.OrderByDescending(p => p.Name),
                "date" => filterDto.OrderDirection == OrderDirection.Ascending
                    ? query.OrderBy(p => p.CreatedDate)
                    : query.OrderByDescending(p => p.CreatedDate),
                "quantity" => filterDto.OrderDirection == OrderDirection.Ascending
                    ? query.OrderBy(p => p.StockQuantity)
                    : query.OrderByDescending(p => p.StockQuantity),
                _ => query
            };

            return await query
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CreatedDate = p.CreatedDate,
                    IsAvailable = p.IsAvailable,
                    ImagePath = p.ImagePath,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    SupplierId = p.SupplierId,
                    SupplierName = p.Supplier.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Where(p => p.CategoryId == categoryId)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CreatedDate = p.CreatedDate,
                    IsAvailable = p.IsAvailable,
                    ImagePath = p.ImagePath,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    SupplierId = p.SupplierId,
                    SupplierName = p.Supplier.Name
                })
                .ToListAsync();
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await _dbContext.Categories.AnyAsync(c => c.Id == categoryId);
        }

        public async Task<bool> SupplierExistsAsync(int supplierId)
        {
            return await _dbContext.Suppliers.AnyAsync(s => s.Id == supplierId);
        }
    }
}
