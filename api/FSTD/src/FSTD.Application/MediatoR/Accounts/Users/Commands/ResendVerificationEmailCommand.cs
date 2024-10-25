using MediatR;

namespace FSTD.Application.MediatoR.Accounts.Users.Commands
{
    public class ResendVerificationEmailCommand : IRequest
    {
        public ResendVerificationEmailCommand(string userEmail, string baseUrl)
        {
            UserEmail = userEmail;
            BaseUrl = baseUrl;
        }
        public string UserEmail { get; set; }
        public string BaseUrl { get; set; }
    }
}
