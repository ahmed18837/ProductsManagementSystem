using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsManagementSystem.Models.DTO.Category;
using ProductsManagementSystem.Services.Interfaces;

namespace ProductsManagementSystem.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        private readonly ICategoryService _categoryService = categoryService;

        [HttpGet("AllCategories")]
        [ResponseCache(Duration = 60)]
        //[Authorize]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var categoriesDto = await _categoryService.GetAllAsync();
                return Ok(categoriesDto);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet("CategoryById/{id:int}")]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var categoryDto = await _categoryService.GetByIdAsync(id);
                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CategoryByName/{Name}")]
        [Authorize]
        public async Task<IActionResult> GetByName(string Name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var categoryDto = await _categoryService.GetByNameAsync(Name);
                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CountProductPerCategory")]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<IActionResult> GetCategoryProductCount()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var categoryProductCountsDto = await _categoryService.GetCategoryProductCountsAsync();
                return Ok(categoryProductCountsDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddCategory")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> AddCategory(CategoryCreateDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _categoryService.AddAsync(createCategoryDto);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                return BadRequest(new { error = errorMessage });
            }
        }

        [HttpPut("UpdateCategory/{id}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _categoryService.UpdateAsync(id, updateDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteCategory/{id}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllCategoriesAddedInLast/{days}Days")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GetAllCategoriesAddedInLastDays(int days)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var categoriesDto = await _categoryService.GetCategoriesAddedInLastDay(days);
                return Ok(categoriesDto);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Filtering")]
        //[Authorize]
        public async Task<IActionResult> GetFilteredCategories([FromQuery] string? name, [FromQuery] bool? isActive, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var categories = await _categoryService.GetFilteredCategoriesAsync(name, isActive, startDate, endDate);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
