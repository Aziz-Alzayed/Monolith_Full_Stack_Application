using MediatR;

namespace FSTD.Application.MediatoR.Accounts.Auth.Commands
{
    public class LogoutCommand : IRequest
    {
        public string UserEmail { get; set; }
        public LogoutCommand(string userEmail)
        {
            UserEmail = userEmail;
        }
    }
}
