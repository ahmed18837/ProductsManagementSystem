using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsManagementSystem.Models.DTO.Customer;
using ProductsManagementSystem.Services.Interfaces;

namespace ProductsManagementSystem.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class CustomerController(ICustomerService customerService) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;

        [HttpGet("AllCustomers")]
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
                var customersDto = await _customerService.GetAllAsync();

                return Ok(customersDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CustomerById/{id}")]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var customerDto = await _customerService.GetByIdAsync(id);

                return Ok(customerDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddCustomer")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Add(CustomerCreateDto customerCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _customerService.AddAsync(customerCreate);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Super Admin")]
        [HttpDelete("DeleteCustomer/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _customerService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Super Admin")]
        [HttpPut("UpdateCustomer/{id}")]
        public async Task<IActionResult> Update(int id, CustomerUpdateDto customerUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _customerService.UpdateAsync(id, customerUpdateDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Super Admin, Admin")]
        [HttpGet("BornAfterYear/{year}")]
        public async Task<IActionResult> GetCustomersBornAfterYear(int year)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var customersDto = await _customerService.GetCustomersBornAfterYearAsync(year);
                return Ok(customersDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Super Admin, Admin")]
        [HttpGet("Filtering")]
        public async Task<IActionResult> GetFilteredCustomers([FromQuery] CustomerFilterDto filterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var customers = await _customerService.GetFilteredCustomersAsync(filterDto);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
