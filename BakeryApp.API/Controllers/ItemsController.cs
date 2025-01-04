using BakeryApp.Application.Commands.ItemCommand;
using BakeryApp.Application.Queries.BakeryItemQueries;
using BakeryApp.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BakeryApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Protect the entire controller; specific roles will be specified per action
    public class ItemsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllBakeryItems")]
        [ProducesResponseType(typeof(IList<BakeryItemDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(Roles = "Admin,Owner,Customer")] // All roles can access
        public async Task<IActionResult> GetAllBakeryItems()
        {
            var query = new GetAllItemsQuery();
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new ApiResponse { Success = false, Message = "Items not found" });
            }

            return Ok(new ApiResponse<List<BakeryItemDto>> { Success = true, Message = "Items Found", Data = result });
        }

        [HttpGet("GetBakeryItemById/{id:guid}")]
        [ProducesResponseType(typeof(BakeryItemDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(Roles = "Admin,Owner,Customer")] // All roles can access
        public async Task<IActionResult> GetBakeryItemById(Guid id)
        {
            var query = new GetItemByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new ApiResponse { Success = false, Message = $"Bakery Item with ID {id} not found" });
            }

            return Ok(new ApiResponse<BakeryItemDto> { Success = true, Message = "Bakery Item Found", Data = result });
        }

        [HttpPost("CreateBakeryItem")]
        [ProducesResponseType(typeof(BakeryItemDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Admin,Owner")] // Only Admin and Owner can create items
        public async Task<IActionResult> CreateBakeryItem([FromBody] CreateItemCommand command)
        {
            if (command == null)
            {
                return BadRequest(new ApiResponse { Success = false, Message = "Invalid Item data" });
            }

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateBakeryItem), new ApiResponse<BakeryItemDto> { Success = true, Message = "Item Created", Data = result });
        }

        [HttpPut("UpdateBakeryItem")]
        [ProducesResponseType(typeof(BakeryItemDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Admin,Owner")] // Only Admin and Owner can update items
        public async Task<IActionResult> UpdateBakeryItem([FromBody] UpdateItemCommand command)
        {
            if (command == null)
            {
                return BadRequest(new ApiResponse { Success = false, Message = "Invalid Items data" });
            }

            try
            {
                var result = await _mediator.Send(command);
                return Ok(new ApiResponse<BakeryItemDto> { Success = true, Message = "Items Updated", Data = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse { Success = false, Message = ex.Message });
            }
        }

        [HttpDelete("DeleteBakeryItem/{id:guid}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(Roles = "Admin")] // Only Admin can delete items
        public async Task<IActionResult> DeleteBakeryItem(Guid id)
        {
            var command = new DeleteItemCommand(id);
            try
            {
                var result = await _mediator.Send(command);
                return Ok(new ApiResponse<bool> { Success = true, Message = "Item Deleted", Data = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse { Success = false, Message = ex.Message });
            }
        }
    }
}
