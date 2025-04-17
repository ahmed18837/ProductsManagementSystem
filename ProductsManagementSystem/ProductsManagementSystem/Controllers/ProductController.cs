using Microsoft.AspNetCore.Mvc;
using ProductsManagementSystem.Models.DTO.Product;
using ProductsManagementSystem.Services.Interfaces;

namespace ProductsManagementSystem.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class ProductController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        [HttpGet("AllProducts")]
        //[Authorize]
        //[ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var productsDto = await _productService.GetAllAsync();
                return Ok(productsDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Super Admin, Admin")]
        [HttpGet("ProductById/{id:int}")]
        //[ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var productDto = await _productService.GetByIdAsync(id);
                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ProductByCategoryId/{categoryId:int}")]
        //[ResponseCache(Duration = 60)]
        //[Authorize(Roles = "Super Admin, Admin")]
        public async Task<IActionResult> GetProductsByCategoryId(int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var productsDto = await _productService.GetProductsByCategoryIdAsync(categoryId);
                return Ok(productsDto);
            }
            catch (Exception ex)
            {
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost("AddProduct")]
        //[Authorize(Roles = "Super Admin, Admin")]
        public async Task<IActionResult> Add([FromForm] CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _productService.AddAsync(createProductDto);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateProduct/{id}")]
        //[Authorize(Roles = "Super Admin, Admin")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateProductDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var productDto = await _productService.GetByIdAsync(id);

                await _productService.UpdateAsync(id, updateDto);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteProduct/{id:int}")]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _productService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetAllProductAddedInLast/{days:int}Days")]
        //[ResponseCache(Duration = 60)]
        //[Authorize(Roles = "Super Admin, Admin")]
        public async Task<IActionResult> GetAllProductAddedInLastDays(int days)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var categoriesDto = await _productService.GetCategoriesAddedInLastDay(days);
                return Ok(categoriesDto);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //[HttpGet("Filtering")]
        ////[ResponseCache(Duration = 60)]
        ////[Authorize]
        //public async Task<IActionResult> GetFilteredProducts([FromQuery] ProductFilterDto filterDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    try
        //    {
        //        var productsDto = await _productService.GetFilteredProductsAsync(filterDto);

        //        return Ok(productsDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
