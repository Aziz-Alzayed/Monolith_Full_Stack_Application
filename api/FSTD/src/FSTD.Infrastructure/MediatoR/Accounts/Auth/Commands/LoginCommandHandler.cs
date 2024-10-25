using FSTD.Application.DTOs.Accounts.Auths;
using FSTD.Application.DTOs.Accounts.Users;
using FSTD.Application.MediatoR.Accounts.Auth.Commands;
using FSTD.Application.MediatoR.Accounts.Auth.Repos;
using FSTD.Infrastructure.MediatoR.Accounts.Services;
using MediatR;

namespace FSTD.Infrastructure.MediatoR.Accounts.Auth.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AccessTokenDto>
    {
        IAuthCommandsRepo authCommandsRepo;
        IAccountService accountService;

        public LoginCommandHandler(IAuthCommandsRepo authCommandsRepo, IAccountService accountService)
        {
            this.authCommandsRepo = authCommandsRepo;
            this.accountService = accountService;
        }

        public async Task<AccessTokenDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await accountService.GetUserByEmailAsync(request.loginRequest.Email);
                if (user == null)
                {
                    throw new InvalidOperationException("User not found.");
                }

                var result = await authCommandsRepo.LoginAsync(user, request.loginRequest.Password);
                var userInfo = new UserInfoDto
                {
                    Id = result.User.Id,
                    Email = result.User.Email,
                    FirstName = result.User.FirstName,
                    LastName = result.User.LastName,
                    EmailConfirmed = result.User.EmailConfirmed
                };
                return new AccessTokenDto
                {
                    UserInfo = userInfo,
                    AccessToken = result.JwtAuth.AccessToken,
                    RefreshToken = result.JwtAuth.RefreshToken
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
