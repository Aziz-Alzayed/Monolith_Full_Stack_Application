using FSTD.Application.DTOs.Accounts.Admins;
using MediatR;

namespace FSTD.Application.MediatoR.Accounts.Admins.Commands
{
    public class AddNewUserCommand : IRequest<UserFullInfoDto>
    {
        public AddNewUserCommand(AddNewUserDto addNewUserDto, string addedBy)
        {
            AddNewUserDto = addNewUserDto;
            AddedBy = addedBy;
        }

        public AddNewUserDto AddNewUserDto { get; set; }
        public string AddedBy { get; set; }
    }
}
