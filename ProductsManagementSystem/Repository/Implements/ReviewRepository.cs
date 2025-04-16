using Microsoft.EntityFrameworkCore;
using ProductsManagementSystem.Data;
using ProductsManagementSystem.Enums;
using ProductsManagementSystem.Models.DTO.Review;
using ProductsManagementSystem.Models.Entities;
using ProductsManagementSystem.Repository.Generic;
using ProductsManagementSystem.Repository.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Repository.Implements
{
    public class ReviewRepository(AppDbContext dbContext) : GenericRepository<Review>(dbContext), IReviewRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync()
        {
            return await _dbContext.Reviews
                .AsNoTracking()
                .Include(r => r.Product)
                .Include(r => r.Customer)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate,
                    ProductId = r.ProductId,
                    ProductName = r.Product != null ? r.Product.Name : "N/A",
                    CustomerId = r.CustomerId,
                    CustomerName = r.Customer != null ? r.Customer.FirstName + " " + r.Customer.LastName : "N/A"
                })
                .ToListAsync();
        }

        public async Task<ReviewDto> GetReviewByIdAsync(int id)
        {
            return await _dbContext.Reviews
                .AsNoTracking()
                .Include(r => r.Product)
                .Include(r => r.Customer)
                .Where(r => r.Id == id)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate,
                    ProductId = r.ProductId,
                    ProductName = r.Product != null ? r.Product.Name : "N/A",
                    CustomerId = r.CustomerId,
                    CustomerName = r.Customer != null ? r.Customer.FirstName + " " + r.Customer.LastName : "N/A"
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsAddedInLastDay(int days)
        {
            if (days <= 0)
            {
                throw new ValidationException("Day must be greater than zero.");
            }
            var fromDate = DateTime.Now.AddDays(-days);

            return await _dbContext.Reviews
                .AsNoTracking()
                .Include(r => r.Product)
                .Include(r => r.Customer)
                .Where(p => p.ReviewDate >= fromDate)
                .OrderByDescending(p => p.ReviewDate)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate,
                    ProductId = r.ProductId,
                    ProductName = r.Product != null ? r.Product.Name : "N/A",
                    CustomerId = r.CustomerId,
                    CustomerName = r.Customer != null ? r.Customer.FirstName + " " + r.Customer.LastName : "N/A"
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ReviewDto>> GetFilteredReviewsAsync(int? minRating, int? maxRating, string orderBy, OrderDirection orderDirection)
        {
            var query = _dbContext.Reviews
                .AsNoTracking()
                .Include(r => r.Product)
                .Include(r => r.Customer)
                .AsQueryable();

            if (minRating.HasValue)
                query = query.Where(r => r.Rating >= minRating.Value);

            if (maxRating.HasValue)
                query = query.Where(r => r.Rating <= maxRating.Value);

            query = orderBy switch
            {
                "rating" => orderDirection == OrderDirection.Ascending
                    ? query.OrderBy(r => r.Rating)
                    : query.OrderByDescending(r => r.Rating),
                "date" => orderDirection == OrderDirection.Ascending
                    ? query.OrderBy(r => r.ReviewDate)
                    : query.OrderByDescending(r => r.ReviewDate),
                _ => throw new ArgumentException($"Invalid orderBy value: {orderBy}")
            };

            return await query
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate,
                    ProductId = r.ProductId,
                    ProductName = r.Product != null ? r.Product.Name : "N/A",
                    CustomerId = r.CustomerId,
                    CustomerName = r.Customer != null ? r.Customer.FirstName + " " + r.Customer.LastName : "N/A"
                })
                .ToListAsync();
        }
    }
}
