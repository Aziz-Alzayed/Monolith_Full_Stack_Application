using FSTD.Application.MediatoR.Accounts.Users.Commands;
using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using MediatR;

namespace FSTD.Infrastructure.MediatoR.Accounts.Users.Commands
{
    public class UpdateUserCommandHander : IRequestHandler<UpdateUserCommand>
    {
        IUserCommandsRepo _usersRepository;
        IUserQueriesRepo _userQueriesRepo;

        public UpdateUserCommandHander(IUserCommandsRepo usersRepository, IUserQueriesRepo userQueriesRepo)
        {
            _usersRepository = usersRepository;
            _userQueriesRepo = userQueriesRepo;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var newInfo = request.UpdateUserDetails;
            var user = await _userQueriesRepo.GetUserByEmailAsync(request.OldEmail);
            if (user == null)
            {
                // Consider whether to reveal that the email address was not found
                throw new NotFoundException("User with the following email has not been found!");
            }

            user.FirstName = newInfo.NewFirstName;
            user.LastName = newInfo.NewLastName;

            await _usersRepository.UpdateUserAsync(user);
        }
    }
}
