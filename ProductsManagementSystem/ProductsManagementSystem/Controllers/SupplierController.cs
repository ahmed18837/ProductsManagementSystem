using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsManagementSystem.Models.DTO.Supplier;
using ProductsManagementSystem.Services.Interfaces;

namespace ProductsManagementSystem.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class SupplierController(ISupplierService supplierService) : ControllerBase
    {
        private readonly ISupplierService _supplierService = supplierService;

        [HttpGet("AllSupplier")]
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
                var supplier = await _supplierService.GetAllAsync();

                return Ok(supplier);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SupplierById/{id:int}")]
        [Authorize(Roles = "Super Admin, Admin")]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var supplier = await _supplierService.GetByIdAsync(id);
                return Ok(supplier);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("AddSupplier")]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<IActionResult> Create(CreateSupplierDto createSupplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _supplierService.AddAsync(createSupplier);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateSupplier/{id:int}")]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<IActionResult> Update(int id, UpdateSupplierDto updateSupplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _supplierService.UpdateAsync(id, updateSupplier);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles = "Super Admin, Admin")]
        [HttpDelete("DeleteSupplier/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _supplierService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Super Admin, Admin")]
        [ResponseCache(Duration = 60)]
        [HttpGet("AllSuppliersAddedInLast/{days:int}Days")]
        public async Task<IActionResult> GetAllReviewsAddedInLastDays(int days)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var suppliersDto = await _supplierService.GetSuppliersAddedInLastDay(days);
                return Ok(suppliersDto);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Filtering")]
        [ResponseCache(Duration = 60)]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<IActionResult> GetFilteredSuppliersAsync([FromQuery] SupplierFilterDto filterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var suppliers = await _supplierService.GetFilteredSuppliersAsync(filterDto);
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
