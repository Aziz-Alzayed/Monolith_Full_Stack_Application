using FSTD.Application.MediatoR.Accounts.Users.Commands;
using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using MediatR;

namespace FSTD.Infrastructure.MediatoR.Accounts.Users.Commands
{
    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand>
    {
        IUserCommandsRepo _usersRepository;
        IUserQueriesRepo _userQueriesRepo;

        public VerifyEmailCommandHandler(IUserCommandsRepo usersRepository, IUserQueriesRepo userQueriesRepo)
        {
            _usersRepository = usersRepository;
            _userQueriesRepo = userQueriesRepo;
        }

        public async Task Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userQueriesRepo.GetUserById(request.UserId);
                if (user == null)
                {
                    throw new NotFoundException("User with the following email has not been found!");
                }
                await _usersRepository.VerifyUserEmailAsync(user, request.VerficationToken);
            }
            catch
            {

                throw;
            }
        }
    }
}
