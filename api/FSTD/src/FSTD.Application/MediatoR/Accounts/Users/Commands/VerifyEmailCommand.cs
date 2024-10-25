using MediatR;

namespace FSTD.Application.MediatoR.Accounts.Users.Commands
{
    public class VerifyEmailCommand : IRequest
    {
        public VerifyEmailCommand(Guid userId, string verficationToken)
        {
            VerficationToken = verficationToken;
            UserId = userId;
        }

        public string VerficationToken { get; set; }
        public Guid UserId { get; set; }
    }
}
