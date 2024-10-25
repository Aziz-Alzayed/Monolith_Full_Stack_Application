using FSTD.Application.DTOs.Accounts.Users;
using MediatR;

namespace FSTD.Application.MediatoR.Accounts.Users.Commands
{
    public class UpdateUserCommand : IRequest
    {
        public UpdateUserCommand(UpdateUserDetailsDto updateUserDetails, string? oldEmail)
        {
            UpdateUserDetails = updateUserDetails;
            OldEmail = oldEmail;
        }

        public UpdateUserDetailsDto UpdateUserDetails { get; set; }
        public string? OldEmail { get; set; }
    }
}
