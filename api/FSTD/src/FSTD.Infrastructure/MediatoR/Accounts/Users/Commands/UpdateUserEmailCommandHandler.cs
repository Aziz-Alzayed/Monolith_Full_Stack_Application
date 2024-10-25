using FSTD.Application.MediatoR.Accounts.Users.Commands;
using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.Application.MediatoR.Accounts.Users.Services;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using MediatR;

namespace FSTD.Infrastructure.MediatoR.Accounts.Users.Commands
{
    public class UpdateUserEmailCommandHandler : IRequestHandler<UpdateUserEmailCommand>
    {
        IUsersService _userService;
        IUserCommandsRepo _usersRepository;
        IUserQueriesRepo _userQueriesRepo;

        public UpdateUserEmailCommandHandler(
            IUsersService userService,
            IUserCommandsRepo usersRepository,
            IUserQueriesRepo userQueriesRepo)
        {
            _userService = userService;
            _usersRepository = usersRepository;
            _userQueriesRepo = userQueriesRepo;
        }

        public async Task Handle(UpdateUserEmailCommand request, CancellationToken cancellationToken)
        {
            var oldEmail = request.OldEmail;
            var newEmail = request.UpdateUserEmail.NewEmail;
            var verificationURL = request.UpdateUserEmail.VerificationUrl;
            _userService.ValidateSentFromEmail(oldEmail);

            var user = await _userQueriesRepo.GetUserByEmailAsync(oldEmail);
            if (user == null)
            {
                throw new NotFoundException("User with the following email has not been found!");
            }

            if (!string.IsNullOrEmpty(newEmail) && newEmail != oldEmail)
            {
                user.Email = newEmail;
                user.EmailConfirmed = false;

                await _userService.SendVerficationTokenAsync(verificationURL, user);
            }
            await _usersRepository.UpdateUserAsync(user);
        }
    }
}
