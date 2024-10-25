using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.Application.MediatoR.Productivity.Tasks.Commands;
using FSTD.Application.MediatoR.Productivity.Tasks.Repos;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using MediatR;

namespace FSTD.Infrastructure.MediatoR.Productivity.Tasks.Commands
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
    {
        private readonly ITaskQueriesRepo _taskQueriesRepo;
        private readonly ITaskCommandsRepo _taskCommandsRepo;
        private readonly IUserQueriesRepo _userQueriesRepo;

        public DeleteTaskCommandHandler(
            ITaskQueriesRepo taskQueriesRepo,
            ITaskCommandsRepo taskCommandsRepo,
            IUserQueriesRepo userQueriesRepo)
        {
            _taskQueriesRepo = taskQueriesRepo;
            _taskCommandsRepo = taskCommandsRepo;
            _userQueriesRepo = userQueriesRepo;
        }

        public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskQueriesRepo.GetTaskByIdAsync(request.ItemId);

            if (task is null)
            {
                throw new NotFoundException($"Task has not been found.");
            }
            var owner = await _userQueriesRepo.GetUserByEmailAsync(request.UserEmail);

            if (owner is null)
            {
                throw new NotFoundException($"User has not been found.");
            }

            if (task.ApplicationUserId != owner.Id)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this task.");
            }

            await _taskCommandsRepo.DeleteTaskAsync(task.Id);
        }
    }
}
