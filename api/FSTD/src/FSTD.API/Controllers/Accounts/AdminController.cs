using FSTD.Application.DTOs.Accounts.Admins;
using FSTD.Application.MediatoR.Accounts.Admins.Commands;
using FSTD.Application.MediatoR.Accounts.Admins.Queries;
using FSTD.DataCore.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSTD.API.Controllers.Accounts
{
    [Authorize(Roles = AppRoles.Admin + "," + AppRoles.Super)]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllUser")]
        [ProducesDefaultResponseType(typeof(IEnumerable<UserFullInfoDto>))]
        public async Task<IActionResult> GetAllUser()
        {
            var users =
                await _mediator.Send(new GetAllUsersQuery(User?.Identity?.Name ?? ""));
            return Ok(users);
        }

        [HttpPost("AddNewUser")]
        [ProducesDefaultResponseType(typeof(UserFullInfoDto))]
        public async Task<IActionResult> AddNewUser(AddNewUserDto addNewUserDto)
        {
            var validationResult = new AddNewUserDtoValidator().Validate(addNewUserDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var result =
                await _mediator.Send(new AddNewUserCommand(addNewUserDto, User?.Identity?.Name ?? ""));
            return Ok(result);
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            var validationResult = new UpdateUserDtoValidator().Validate(updateUserDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            await _mediator.Send(new UpdateUserByAdminCommand(updateUserDto, User?.Identity?.Name ?? ""));
            return Ok();
        }

        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("userId is Empty");
            }

            await _mediator.Send(new DeleteUserByIdCommand(userId, User?.Identity?.Name ?? ""));
            return Ok("User has been deleted successfully.");
        }

    }
}
