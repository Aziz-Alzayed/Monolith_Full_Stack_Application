using FSTD.Application.DTOs.Accounts.Admins;
using MediatR;

namespace FSTD.Application.MediatoR.Accounts.Admins.Commands
{
    public class UpdateUserByAdminCommand : IRequest
    {
        public UpdateUserByAdminCommand(UpdateUserDto editUserDto, string updatedBy)
        {
            EditUserDto = editUserDto;
            UpdatedBy = updatedBy;
        }

        public UpdateUserDto EditUserDto { get; set; }
        public string UpdatedBy { get; set; }
    }
}
