using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsManagementSystem.Enums;
using ProductsManagementSystem.Models.DTO.Review;
using ProductsManagementSystem.Repository.Interfaces;
using ProductsManagementSystem.Services.Interfaces;

namespace ProductsManagementSystem.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class ReviewController(IReviewService reviewService, IReviewRepository review, IMapper mapper) : ControllerBase
    {
        private readonly IReviewService _reviewService = reviewService;
        private readonly IReviewRepository _review = review;

        [HttpGet("AllReviews")]
        [ResponseCache(Duration = 60)]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var reviewDto = await _reviewService.GetAllAsync();
                return Ok(reviewDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ReviewById/{id:int}")]
        [Authorize(Roles = "Super Admin, Admin")]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var reviewDto = await _reviewService.GetByIdAsync(id);
                return Ok(reviewDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddReview")]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<IActionResult> Create(CreateReviewDto createReview)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _reviewService.AddAsync(createReview);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpPut("UpdateReview/{id:int}")]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<IActionResult> Update(int id, UpdateReviewDto updateReview)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _reviewService.UpdateAsync(id, updateReview);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteReview/{id:int}")]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _reviewService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("AllReviewsAddedInLast/{days}Days")]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<IActionResult> GetAllReviewsAddedInLastDays(int days)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var reviews = await _review.GetReviewsAddedInLastDay(days);

                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Filtering")]
        [ResponseCache(Duration = 60)]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<IActionResult> GetFilteredReviewsAsync([FromQuery] int? minRating, [FromQuery] int? maxRating, [FromQuery] string orderBy, [FromQuery] OrderDirection orderDirection)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var reviews = await _review.GetFilteredReviewsAsync(minRating, maxRating, orderBy, orderDirection);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
