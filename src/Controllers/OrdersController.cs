using System.Security.Claims;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Services.order;
using Backend_Teamwork.src.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Backend_Teamwork.src.DTO.OrderDTO;

namespace Backend_Teamwork.src.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService service)
        {
            _orderService = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<OrderReadDto>>> GetOrders(
            [FromQuery] PaginationOptions paginationOptions
        )
        {
            var orderList = await _orderService.GetAllAsync(paginationOptions);
            var totalCount = await _orderService.GetCountAsync();
            var orderResponse = new { OrderList = orderList, TotalCount = totalCount };
            return Ok(orderResponse);
        }

        [HttpGet("my-orders")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<List<OrderReadDto>>> GetMyOrders(
            PaginationOptions paginationOptions
        )
        {
            var authClaims = HttpContext.User;
            var userId = authClaims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var convertedUserId = new Guid(userId);

            var orderList = await _orderService.GetAllAsync(paginationOptions, convertedUserId);
            var totalCount = await _orderService.GetCountAsync();
            var orderResponse = new { OrderList = orderList, TotalCount = totalCount };
            return Ok(orderResponse);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<OrderReadDto>> GetOrderById([FromRoute] Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);
            return Ok(order);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<OrderReadDto>> AddOrder([FromBody] OrderCreateDto createDto)
        {
            var authenticateClaims = HttpContext.User;
            var userId = authenticateClaims
                .FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!
                .Value;
            var userGuid = new Guid(userId);

            var orderCreated = await _orderService.CreateOneAsync(userGuid, createDto);

            return CreatedAtAction(
                nameof(GetOrderById),
                new { id = orderCreated.Id },
                orderCreated
            );
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrderReadDto>> UpdateOrder(
            [FromRoute] Guid id,
            [FromBody] OrderUpdateDto updateDto
        )
        {
            var updatedOrder = await _orderService.UpdateOneAsync(id, updateDto);
            return Ok(updatedOrder);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteOrder(Guid id)
        {
            await _orderService.DeleteOneAsync(id);
            return NoContent();
        }
    }
}
