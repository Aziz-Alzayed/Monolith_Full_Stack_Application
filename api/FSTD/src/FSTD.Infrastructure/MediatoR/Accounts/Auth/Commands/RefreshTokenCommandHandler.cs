using FSTD.Application.DTOs.Accounts.Auths;
using FSTD.Application.MediatoR.Accounts.Auth.Commands;
using FSTD.Application.MediatoR.Accounts.Auth.Repos;
using MediatR;

namespace FSTD.Infrastructure.MediatoR.Accounts.Auth.Commands
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshAccessTokenDto>
    {
        IAuthCommandsRepo _authCommandsRepo;

        public RefreshTokenCommandHandler(IAuthCommandsRepo authCommandsRepo)
        {
            _authCommandsRepo = authCommandsRepo;
        }

        public async Task<RefreshAccessTokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _authCommandsRepo.RefreshTokenAsync(request.AccessToken, request.RefreshToken);
                return new RefreshAccessTokenDto { AccessToken = result.AccessToken, RefreshToken = result.RefreshToken };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
