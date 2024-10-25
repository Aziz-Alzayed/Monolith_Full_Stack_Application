using FSTD.Application.MediatoR.Accounts.Auth.Commands;
using FSTD.Application.MediatoR.Accounts.Auth.Repos;
using MediatR;

namespace FSTD.Infrastructure.MediatoR.Accounts.Auth.Commands
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        IAuthCommandsRepo _authCommandsRepo;

        public LogoutCommandHandler(IAuthCommandsRepo authCommandsRepo)
        {
            _authCommandsRepo = authCommandsRepo;
        }

        public Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _authCommandsRepo.LogoutUser(request.UserEmail);
                return Task.CompletedTask;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
