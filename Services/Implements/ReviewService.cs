using AutoMapper;
using ProductsManagementSystem.Enums;
using ProductsManagementSystem.Models.DTO.Review;
using ProductsManagementSystem.Models.Entities;
using ProductsManagementSystem.Repository.Interfaces;
using ProductsManagementSystem.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagementSystem.Services.Implements
{
    public class ReviewService(IReviewRepository reviewRepository, IMapper mapper) : IReviewService
    {
        private readonly IReviewRepository _reviewRepository = reviewRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<ReviewDto>> GetAllAsync()
        {
            var reviews = await _reviewRepository.GetAllReviewsAsync();
            if (reviews == null || !reviews.Any())
                throw new Exception("Not there Reviews");

            return reviews;
        }

        public async Task<ReviewDto> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ValidationException("Id must be greater than zero");

            var review = await _reviewRepository.GetReviewByIdAsync(id) ??
                throw new KeyNotFoundException("Review not found");
            return review;
        }

        public async Task AddAsync(CreateReviewDto createReviewDto)
        {
            if (createReviewDto == null)
            {
                throw new Exception("Review Can not be null");
            }

            var review = _mapper.Map<Review>(createReviewDto);
            await _reviewRepository.AddAsync(review);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0) throw new ValidationException("Id must be greater than zero");

            var review = await _reviewRepository.GetByIdAsync(id) ??
               throw new KeyNotFoundException("Review not found");

            await _reviewRepository.DeleteAsync(id);
        }

        public async Task UpdateAsync(int id, UpdateReviewDto updateReviewDto)
        {
            var review = await _reviewRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("Review not found");


            if (review.Rating == updateReviewDto.Rating
             && review.ProductId == updateReviewDto.ProductId
             && review.CustomerId == updateReviewDto.CustomerId)
            {
                throw new Exception("No changes detected. Review remains unchanged");
            }

            _mapper.Map(updateReviewDto, review);

            await _reviewRepository.UpdateAsync(id, review);
        }

        public async Task<IEnumerable<ReviewDto>> GetFilteredReviewsAsync(int? minRating, int? maxRating, string orderBy, OrderDirection orderDirection)
        {
            if (minRating < 0 || maxRating < 0) throw new Exception("Some Inputs Invalid!");

            var reviews = await _reviewRepository.GetFilteredReviewsAsync(minRating, maxRating, orderBy, orderDirection);

            if (reviews == null || !reviews.Any())
                throw new Exception("Not there Reviews");

            return reviews;
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsAddedInLastDay(int days)
        {
            if (days < 0) throw new Exception("Invalid Value");

            var reviews = await _reviewRepository.GetReviewsAddedInLastDay(days);

            if (reviews == null || !reviews.Any())
                throw new Exception("Not there Reviews");

            return reviews;
        }
    }
}
