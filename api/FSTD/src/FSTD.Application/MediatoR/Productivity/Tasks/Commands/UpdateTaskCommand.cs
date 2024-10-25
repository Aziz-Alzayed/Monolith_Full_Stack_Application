using FSTD.Application.DTOs.Productivity.Tasks;
using MediatR;

namespace FSTD.Application.MediatoR.Productivity.Tasks.Commands
{
    public class UpdateTaskCommand : IRequest
    {
        public UpdateTaskCommand(TaskDto updatedItem, string userEmail)
        {
            UpdatedItem = updatedItem;
            UserEmail = userEmail;
        }

        public TaskDto UpdatedItem { get; set; }
        public string UserEmail { get; set; }
    }
}
