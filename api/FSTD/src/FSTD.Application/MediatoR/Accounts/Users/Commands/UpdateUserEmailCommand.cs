using FSTD.Application.DTOs.Accounts.Users;
using MediatR;

namespace FSTD.Application.MediatoR.Accounts.Users.Commands
{
    public class UpdateUserEmailCommand : IRequest
    {
        public UpdateUserEmailDto UpdateUserEmail;
        public string OldEmail { get; set; }
        public string BaseUrl { get; set; }

        public UpdateUserEmailCommand(UpdateUserEmailDto updateUserEmail, string baseUrl, string oldEmail)
        {
            UpdateUserEmail = updateUserEmail ?? throw new ArgumentNullException(nameof(updateUserEmail));
            BaseUrl = baseUrl ?? throw new ArgumentNullException(nameof(oldEmail));
            OldEmail = oldEmail ?? throw new ArgumentNullException(nameof(baseUrl));
        }
    }
}
