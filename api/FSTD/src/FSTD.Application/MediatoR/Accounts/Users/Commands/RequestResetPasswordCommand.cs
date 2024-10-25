using FSTD.Application.DTOs.Accounts.Users;
using MediatR;

namespace FSTD.Application.MediatoR.Accounts.Users.Commands
{
    public class RequestResetPasswordCommand : IRequest
    {
        public RequestResetPasswordDto requestResetPasswordDto { get; set; }

        public RequestResetPasswordCommand(RequestResetPasswordDto requestResetPasswordDto)
        {
            this.requestResetPasswordDto = requestResetPasswordDto;
        }
    }
}
