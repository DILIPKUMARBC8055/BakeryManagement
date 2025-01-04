// Controllers/UserController.cs
using BakeryApp.Application.Commands.UserCommand;
using BakeryApp.Application.Queries.UserQueries;
using BakeryApp.Application.Response;
using BakeryApp.Application.Services;
using BakeryApp.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BakeryApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly PasswordService _passwordService;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
            _passwordService = new PasswordService();
        }


        [HttpGet("GetAllUsers")]
        [ProducesResponseType(typeof(IList<OrderDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new ApiResponse { Success = false, Message = "Users not found" });
            }

            return Ok(new ApiResponse<List<UserDto>> { Success = true, Message = "Users Found", Data = result });
        }


        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var query = new GetUserByIdQuery(id);
            var user = await _mediator.Send(query);

            if (user == null)
            {
                return NotFound(new ApiResponse { Success = false, Message = "User not found" });
            }

            return Ok(new ApiResponse<UserDto> { Success = true, Message = "User Found", Data = user });
        }


        [HttpPost]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            if (command == null)
            {
                return BadRequest(new ApiResponse { Success = false, Message = "Invalid user data" });
            }
            command.Password = _passwordService.HashPassword(command.Password);



            var user = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, new ApiResponse<UserDto> { Success = true, Message = "User Created", Data = user });
        }


        [HttpPost("Login")]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new ApiResponse { Success = false, Message = "Invalid credentials" });
            }
        }
    }
}
