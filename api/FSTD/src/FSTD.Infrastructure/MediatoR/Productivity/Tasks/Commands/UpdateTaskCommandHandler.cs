using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.Application.MediatoR.Productivity.Tasks.Commands;
using FSTD.Application.MediatoR.Productivity.Tasks.Repos;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using MediatR;

namespace FSTD.Infrastructure.MediatoR.Productivity.Tasks.Commands
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand>
    {
        private readonly ITaskQueriesRepo _taskQueriesRepo;
        private readonly ITaskCommandsRepo _taskCommandsRepo;
        private readonly IUserQueriesRepo _userQueriesRepo;

        public UpdateTaskCommandHandler(ITaskQueriesRepo taskQueriesRepo, ITaskCommandsRepo taskCommandsRepo, IUserQueriesRepo userQueriesRepo)
        {
            _taskQueriesRepo = taskQueriesRepo;
            _taskCommandsRepo = taskCommandsRepo;
            _userQueriesRepo = userQueriesRepo;
        }

        public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var dto = request.UpdatedItem;
            // Retrieve the task by its ID
            var task = await _taskQueriesRepo.GetTaskByIdAsync(dto.Id);

            if (task == null)
            {
                throw new NotFoundException($"Task has not been found.");
            }

            var owner = await _userQueriesRepo.GetUserByEmailAsync(request.UserEmail);

            if (owner == null)
            {
                throw new NotFoundException($"User hass not been not found.");
            }

            if (task.ApplicationUserId != owner.Id)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this task.");
            }

            task.Name = dto.Name ?? task.Name;
            task.Description = dto.Description ?? task.Description;
            task.IsDone = dto.IsDone;
            task.ValidUntil = dto.ValidUntil;

            // Save the updated task to the database
            await _taskCommandsRepo.UpdateTaskAsync(task);
        }
    }
}
