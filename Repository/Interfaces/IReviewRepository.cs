using ProductsManagementSystem.Enums;
using ProductsManagementSystem.Models.DTO.Review;
using ProductsManagementSystem.Models.Entities;
using ProductsManagementSystem.Repository.Generic;

namespace ProductsManagementSystem.Repository.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();
        Task<ReviewDto> GetReviewByIdAsync(int id);
        Task<IEnumerable<ReviewDto>> GetReviewsAddedInLastDay(int days);

        Task<IEnumerable<ReviewDto>> GetFilteredReviewsAsync(int? minRating, int? maxRating, string orderBy, OrderDirection orderDirection);
    }
}
