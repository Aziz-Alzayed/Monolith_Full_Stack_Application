using FSTD.Application.DTOs.Productivity.Tasks;
using MediatR;

namespace FSTD.Application.MediatoR.Productivity.Tasks.Queries
{
    public class GetAllTasksQuery : IRequest<IList<TaskDto>>
    {
        public GetAllTasksQuery(string userEmail)
        {
            UserEmail = userEmail;
        }

        public string UserEmail { get; set; }
    }
}
