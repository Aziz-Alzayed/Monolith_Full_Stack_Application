using MediatR;

namespace FSTD.Application.MediatoR.Accounts.Admins.Commands
{
    public class DeleteUserByIdCommand : IRequest
    {
        public DeleteUserByIdCommand(Guid userId, string editedBy)
        {
            UserId = userId;
            EditedBy = editedBy;
        }

        public Guid UserId { get; set; }
        public string EditedBy { get; set; }
    }
}
