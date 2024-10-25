using AutoMapper;
using FSTD.Application.DTOs.Productivity.Tasks;
using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.Application.MediatoR.Productivity.Tasks.Queries;
using FSTD.Application.MediatoR.Productivity.Tasks.Repos;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using MediatR;

namespace FSTD.Infrastructure.MediatoR.Productivity.Tasks.Queries
{
    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IList<TaskDto>>
    {
        private readonly ITaskQueriesRepo _taskQueriesRepo;
        private readonly IUserQueriesRepo _userQueriesRepo;
        private readonly IMapper _mapper;

        public GetAllTasksQueryHandler(
            ITaskQueriesRepo taskQueriesRepo,
            IUserQueriesRepo userQueriesRepo,
            IMapper mapper)
        {
            _taskQueriesRepo = taskQueriesRepo;
            _userQueriesRepo = userQueriesRepo;
            _mapper = mapper;
        }

        public async Task<IList<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            // Fetch the user by email
            var user = await _userQueriesRepo.GetUserByEmailAsync(request.UserEmail);
            if (user == null)
            {
                throw new NotFoundException("User with the following email has not been found!");
            }

            // Fetch all tasks by user ID
            var listOfTasks = await _taskQueriesRepo.GetAllTasksByUserIdAsync(user.Id);

            return _mapper.Map<List<TaskDto>>(listOfTasks);
        }
    }
}
