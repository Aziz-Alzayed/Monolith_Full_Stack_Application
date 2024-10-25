using FSTD.Application.MediatoR.Accounts.Users.Commands;
using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using FSTD.Infrastructure.MediatoR.Accounts.Services;
using MediatR;

namespace FSTD.Infrastructure.MediatoR.Accounts.Users.Commands
{
    public class ResendVerificationEmailCommandHandler : IRequestHandler<ResendVerificationEmailCommand>
    {
        IUserCommandsRepo _usersCommandsRepo;
        IUserQueriesRepo _userQueriesRepo;
        IAccountEmailSender _accountEmail;

        public ResendVerificationEmailCommandHandler(
            IUserCommandsRepo usersCommandsRepo,
            IUserQueriesRepo userQueriesRepo,
            IAccountEmailSender accountEmail)
        {
            _usersCommandsRepo = usersCommandsRepo;
            _userQueriesRepo = userQueriesRepo;
            _accountEmail = accountEmail;
        }

        public async Task Handle(ResendVerificationEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userQueriesRepo.GetUserByEmailAsync(request.UserEmail);
            if (user == null)
            {
                throw new NotFoundException("Email has been not found.");
            }
            var token = await _usersCommandsRepo.GenerateEmailConfirmationTokenAsync(user);
            await _accountEmail.SendVerificationEmailAsync(request.BaseUrl, token, user);
        }
    }
}
