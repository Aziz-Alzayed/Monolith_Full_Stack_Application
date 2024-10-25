using FSTD.Application.DTOs.Accounts.Users;
using MediatR;

namespace FSTD.Application.MediatoR.Accounts.Users.Commands
{
    public class UpdateUserPasswordCommand : IRequest
    {
        public UpdateUserPasswordCommand(UpdateUserPasswordDto updateUserPassword, string userEmail)
        {
            this.updateUserPassword = updateUserPassword;
            UserEmail = userEmail;
        }

        public UpdateUserPasswordDto updateUserPassword { get; set; }
        public string UserEmail { get; set; }
    }
}
