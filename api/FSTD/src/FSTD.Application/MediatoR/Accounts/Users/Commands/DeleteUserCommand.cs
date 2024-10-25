using MediatR;

namespace FSTD.Application.MediatoR.Accounts.Users.Commands
{
    public class DeleteUserCommand : IRequest
    {
        public DeleteUserCommand(string userEmail)
        {
            UserEmail = userEmail;
        }

        public string UserEmail { get; set; }
    }
}
