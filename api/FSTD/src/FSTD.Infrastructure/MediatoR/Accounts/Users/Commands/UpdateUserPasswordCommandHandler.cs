using FSTD.Application.MediatoR.Accounts.Users.Commands;
using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using MediatR;

namespace FSTD.Infrastructure.MediatoR.Accounts.Users.Commands
{
    public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand>
    {
        IUserQueriesRepo _userQueriesRepo;
        IUserCommandsRepo _userCommandsRepo;

        public UpdateUserPasswordCommandHandler(IUserQueriesRepo userQueriesRepo, IUserCommandsRepo userCommandsRepo)
        {
            _userQueriesRepo = userQueriesRepo;
            _userCommandsRepo = userCommandsRepo;
        }

        public async Task Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userQueriesRepo.GetUserByEmailAsync(request.UserEmail);
            if (user == null)
            {
                throw new NotFoundException("User with the following email has not been found!");
            }
            await _userCommandsRepo.UpdateUserPasswordAsync(user, request.updateUserPassword.OldPassword, request.updateUserPassword.NewPassword);
        }
    }
}
