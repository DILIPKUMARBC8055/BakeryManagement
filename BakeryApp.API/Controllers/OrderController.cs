using BakeryApp.Application.Commands.OrderCommand;
using BakeryApp.Application.Queries.OrderQueries;
using BakeryApp.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BakeryApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // Get all orders
        [HttpGet("GetAllOrders")]
        [ProducesResponseType(typeof(IList<OrderDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(Roles = "Admin,Owner,Customer")]
        public async Task<IActionResult> GetAllOrders()
        {
            var query = new GetAllOrdersQuery();
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new ApiResponse { Success = false, Message = "Orders not found" });
            }

            return Ok(new ApiResponse<List<OrderDto>> { Success = true, Message = "Orders Found", Data = result });
        }


        // Get order by ID
        [HttpGet("GetOrderById/{id:guid}")]
        [ProducesResponseType(typeof(OrderDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(Roles = "Admin,Owner,Customer")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var query = new GetOrderByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new ApiResponse { Success = false, Message = $"Order with ID {id} not found" });
            }

            return Ok(new ApiResponse<OrderDto> { Success = true, Message = "Order Found", Data = result });
        }


        // Create a new order
        [HttpPost("CreateOrder")]
        [ProducesResponseType(typeof(OrderDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            if (command == null)
            {
                return BadRequest(new ApiResponse { Success = false, Message = "Invalid order data" });
            }

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetOrderById), new { id = result.Id }, new ApiResponse<OrderDto> { Success = true, Message = "Order Created", Data = result });
        }


        // Update an order
        [HttpPut("UpdateOrder")]
        [ProducesResponseType(typeof(OrderDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            if (command == null)
            {
                return BadRequest(new ApiResponse { Success = false, Message = "Invalid order data" });
            }

            try
            {
                var result = await _mediator.Send(command);
                return Ok(new ApiResponse<OrderDto> { Success = true, Message = "Order Updated", Data = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse { Success = false, Message = ex.Message });
            }
        }


        // Delete an order
        [HttpDelete("DeleteOrder/{id:guid}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var command = new DeleteOrderCommand(id);
            try
            {
                var result = await _mediator.Send(command);
                return Ok(new ApiResponse<bool> { Success = true, Message = "Order Deleted", Data = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse { Success = false, Message = ex.Message });
            }
        }
    }
}
