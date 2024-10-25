using MediatR;

namespace FSTD.Application.MediatoR.Accounts.Users.Commands
{
    public class ResetForgetPasswordCommand : IRequest
    {
        public ResetForgetPasswordCommand(string email, string token, string newPassword)
        {
            Email = email;
            Token = token;
            NewPassword = newPassword;
        }

        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
