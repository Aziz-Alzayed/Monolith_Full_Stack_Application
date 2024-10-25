using FSTD.Application.MediatoR.Accounts.Users.Commands;
using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using FSTD.Infrastructure.MediatoR.Accounts.Services;
using MediatR;

namespace FSTD.Infrastructure.MediatoR.Accounts.Users.Commands
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand>
    {
        IUserQueriesRepo _userQueriesRepo;
        IUserCommandsRepo _userCommandsRepo;
        IAccountEmailSender _accountEmail;

        public ForgetPasswordCommandHandler(
            IUserQueriesRepo userQueriesRepo,
            IUserCommandsRepo userCommandsRepo,
            IAccountEmailSender accountEmail)
        {
            _userQueriesRepo = userQueriesRepo;
            _userCommandsRepo = userCommandsRepo;
            _accountEmail = accountEmail;
        }

        public async Task Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userQueriesRepo.GetUserByEmailAsync(request.Email);
                if (user == null)
                {
                    throw new NotFoundException("User with the following email has not been found!");
                }

                // Generate the password reset token
                var token = await _userCommandsRepo.GeneratePasswordResetTokenAsync(user);
                await _accountEmail.SendPasswordResetEmailAsync(request.ResetUrl, token, user);
            }
            catch
            {

                throw;
            }
        }
    }
}
