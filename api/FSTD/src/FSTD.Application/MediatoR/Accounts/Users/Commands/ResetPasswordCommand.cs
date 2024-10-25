using MediatR;

namespace FSTD.Application.MediatoR.Accounts.Users.Commands
{
    public class ResetPasswordCommand : IRequest
    {
        public string Email { get; }
        public string Token { get; }
        public string NewPassword { get; }

        public ResetPasswordCommand(string email, string token, string newPassword)
        {
            Email = email;
            Token = token;
            NewPassword = newPassword;
        }
    }
}
