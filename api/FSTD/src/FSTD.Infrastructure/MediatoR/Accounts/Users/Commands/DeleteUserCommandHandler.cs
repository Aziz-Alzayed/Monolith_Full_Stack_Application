using FSTD.Application.MediatoR.Accounts.Users.Commands;
using FSTD.Application.MediatoR.Accounts.Users.Repos;
using MediatR;

namespace FSTD.Infrastructure.MediatoR.Accounts.Users.Commands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserCommandsRepo _usersRepository;
        public DeleteUserCommandHandler(IUserCommandsRepo usersRepository)
        {
            _usersRepository = usersRepository;
        }
        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _usersRepository.DeleteUserAsync(request.UserEmail);
            }
            catch
            {

                throw;
            }
        }
    }
}
