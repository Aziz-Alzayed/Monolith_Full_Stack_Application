using FSTD.Application.DTOs.Accounts.Auths;
using MediatR;

namespace FSTD.Application.MediatoR.Accounts.Auth.Commands
{
    public class LoginCommand : IRequest<AccessTokenDto>
    {
        public LoginCommand(LoginRequestDto loginRequest)
        {
            this.loginRequest = loginRequest;
        }

        public LoginRequestDto loginRequest { get; set; }
    }
}
