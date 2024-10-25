using FSTD.Application.DTOs.Productivity.Tasks;
using MediatR;

namespace FSTD.Application.MediatoR.Productivity.Tasks.Commands
{
    public class AddTaskCommand : IRequest<TaskDto>
    {
        public AddTaskCommand(AddTaskDto addTaskDto, string userEmail)
        {
            this.addTaskDto = addTaskDto;
            UserEmail = userEmail;
        }

        public AddTaskDto addTaskDto { get; set; }
        public string UserEmail { get; set; }
    }
}
