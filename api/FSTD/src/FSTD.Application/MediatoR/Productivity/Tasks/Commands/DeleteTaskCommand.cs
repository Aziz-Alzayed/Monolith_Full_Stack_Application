using MediatR;

namespace FSTD.Application.MediatoR.Productivity.Tasks.Commands
{
    public class DeleteTaskCommand : IRequest
    {
        public DeleteTaskCommand(string userEmail, Guid itemId)
        {
            UserEmail = userEmail;
            ItemId = itemId;
        }

        public string UserEmail { get; set; }
        public Guid ItemId { get; set; }
    }
}
