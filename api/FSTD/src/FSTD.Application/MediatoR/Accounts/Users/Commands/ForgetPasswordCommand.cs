using MediatR;

namespace FSTD.Application.MediatoR.Accounts.Users.Commands
{
    public class ForgetPasswordCommand : IRequest
    {
        public ForgetPasswordCommand(string email, string resetUrl)
        {
            Email = email;
            ResetUrl = resetUrl;
        }

        public string Email { get; set; }
        public string ResetUrl { get; set; }
    }
}
