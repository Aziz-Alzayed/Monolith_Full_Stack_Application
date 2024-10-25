using FSTD.Application.DTOs.Accounts.Admins;
using FSTD.Application.DTOs.Productivity.Tasks;
using FSTD.Application.MediatoR.Productivity.Tasks.Commands;
using FSTD.Application.MediatoR.Productivity.Tasks.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSTD.API.Controllers.Productivity
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController(
        IMediator _mediator
        ) : ControllerBase
    {
        [HttpGet("GetAllTasks")]
        [ProducesDefaultResponseType(typeof(IEnumerable<TaskDto>))]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _mediator.Send(new GetAllTasksQuery(User?.Identity?.Name ?? ""));
            return Ok(tasks);
        }
        [HttpPost]
        [ProducesDefaultResponseType(typeof(TaskDto))]
        public async Task<IActionResult> Post(AddTaskDto item)
        {
            var validationResult = new AddTaskDtoValidator().Validate(item);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var task = await _mediator.Send(new AddTaskCommand(item, User?.Identity?.Name ?? ""));
            return Ok(task);
        }
        [HttpDelete("DeleteTask/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("id must not be nul or empty");
            }
            await _mediator.Send(new DeleteTaskCommand(User?.Identity?.Name ?? "", id));
            return Ok();
        }
        [HttpPut]
        [ProducesDefaultResponseType(typeof(IEnumerable<UserFullInfoDto>))]
        public async Task<IActionResult> Put(TaskDto item)
        {
            var validationResult = new TaskDtoValidator().Validate(item);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            await _mediator.Send(new UpdateTaskCommand(item, User?.Identity?.Name ?? ""));
            return Ok();
        }
    }
}
