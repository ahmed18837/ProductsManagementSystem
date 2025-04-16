using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsManagementSystem.Models.DTO.Order;
using ProductsManagementSystem.Services.Interfaces;

namespace ProductsManagementSystem.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        [HttpGet("AllOrders")]
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
                var ordersDto = await _orderService.GetAllOrdersAsync();
                return Ok(ordersDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Super Admin, Admin")]
        [ResponseCache(Duration = 60)]
        [HttpGet("OrderById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var orderDto = await _orderService.GetOrderByIdAsync(id);
                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddOrder")]
        [Authorize]
        public async Task<IActionResult> Add(OrderCreateDto orderCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _orderService.AddAsync(orderCreateDto);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("UpdateOrder/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _orderService.UpdateAsync(id, updateDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize]
        [HttpDelete("DeleteOrder/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _orderService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Super Admin, Admin")]
        [HttpGet("AllOrdersAddedInLast/{days:int}Days")]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetAllProductAddedInLastDays(int days)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var categoriesDto = await _orderService.GetOrdersAddedInLastDay(days);
                return Ok(categoriesDto);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Super Admin, Admin")]
        [HttpGet("Filtering")]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetFilteredOrders([FromQuery] OrderFilterDto filterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var ordersDto = await _orderService.GetFilteredOrdersAsync(filterDto);

                return Ok(ordersDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
