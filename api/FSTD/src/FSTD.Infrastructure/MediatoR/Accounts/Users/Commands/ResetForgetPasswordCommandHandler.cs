using FSTD.Application.MediatoR.Accounts.Users.Commands;
using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using MediatR;

namespace FSTD.Infrastructure.MediatoR.Accounts.Users.Commands
{
    public class ResetForgetPasswordCommandHandler : IRequestHandler<ResetForgetPasswordCommand>
    {
        IUserCommandsRepo _usersCommandsRepo;
        IUserQueriesRepo _userQueriesRepo;

        public ResetForgetPasswordCommandHandler(
            IUserCommandsRepo usersCommandsRepo,
            IUserQueriesRepo userQueriesRepo)
        {
            _usersCommandsRepo = usersCommandsRepo;
            _userQueriesRepo = userQueriesRepo;
        }

        public async Task Handle(ResetForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userQueriesRepo.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                throw new NotFoundException("User with the following email has not been found!");
            }
            await _usersCommandsRepo.ResetUserPasswordAsync(user, request.Token, request.NewPassword);
        }
    }
}
