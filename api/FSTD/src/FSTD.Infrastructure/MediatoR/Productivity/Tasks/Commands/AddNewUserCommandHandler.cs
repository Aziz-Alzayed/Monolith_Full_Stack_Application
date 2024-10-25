using AutoMapper;
using FSTD.Application.DTOs.Productivity.Tasks;
using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.Application.MediatoR.Productivity.Tasks.Commands;
using FSTD.Application.MediatoR.Productivity.Tasks.Repos;
using FSTD.DataCore.Models.ProductivityModels;
using MediatR;

namespace FSTD.Infrastructure.MediatoR.Productivity.Tasks.Commands
{
    public class AddTaskCommandHandler : IRequestHandler<AddTaskCommand, TaskDto>
    {
        private readonly ITaskCommandsRepo _taskCommandsRepo;
        private readonly IUserQueriesRepo _userQueriesRepo;
        private readonly IMapper _mapper;

        public AddTaskCommandHandler(ITaskCommandsRepo taskCommandsRepo, IUserQueriesRepo userQueriesRepo, IMapper mapper)
        {
            _taskCommandsRepo = taskCommandsRepo;
            _userQueriesRepo = userQueriesRepo;
            _mapper = mapper;
        }

        public async Task<TaskDto> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            var addedBy = await _userQueriesRepo.GetUserByEmailAsync(request.UserEmail);

            if (addedBy is null)
                throw new UnauthorizedAccessException();

            var dto = request.addTaskDto;
            var newTask = new TasksModel()
            {
                ApplicationUserId = addedBy.Id,
                ApplicationUser = addedBy,
                Description = dto.Description,
                IsDone = dto.IsDone,
                Name = dto.Name,
                ValidUntil = dto.ValidUntil
            };
            var result = await _taskCommandsRepo.AddTaskAsync(newTask);

            return _mapper.Map<TaskDto>(result);
        }
    }
}
