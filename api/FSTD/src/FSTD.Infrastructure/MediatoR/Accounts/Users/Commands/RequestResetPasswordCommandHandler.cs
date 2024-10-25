using FSTD.Application.MediatoR.Accounts.Users.Commands;
using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.DataCore.Models.Users;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using FSTD.Infrastructure.EmailServices;
using MediatR;
using System.Web;

namespace FSTD.Infrastructure.MediatoR.Accounts.Users.Commands
{
    public class RequestResetPasswordCommandHandler : IRequestHandler<RequestResetPasswordCommand>
    {
        IUserCommandsRepo _userCommandsRepo;
        IUserQueriesRepo _userQueriesRepo;
        IEmailService _emailService;

        public RequestResetPasswordCommandHandler(
            IUserCommandsRepo userCommandsRepo,
            IUserQueriesRepo userQueriesRepo,
            IEmailService emailService)
        {
            _userCommandsRepo = userCommandsRepo;
            _userQueriesRepo = userQueriesRepo;
            _emailService = emailService;
        }

        public async Task Handle(RequestResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var dto = request.requestResetPasswordDto;
            var user = await _userQueriesRepo.GetUserByEmailAsync(dto.Email);
            if (user == null)
            {
                throw new NotFoundException("Email has been not found.");
            }
            var token = await _userCommandsRepo.GeneratePasswordResetTokenAsync(user);
            await SendNotificationEmail(user, token, dto.ResetURL);
        }

        private async Task SendNotificationEmail(ApplicationUser user, string token, string resetURL)
        {
            // Create a reset link to be sent to the user
            var encodedToken = HttpUtility.UrlEncode(token);
            var resetLink = $"{resetURL}/?token={encodedToken}&email={user.Email}";

            // Send the reset link to the user's email
            var emailSubject = "Reset Your Password";
            var emailBody = $"Please reset your password by clicking <a href='{resetLink}'>here</a>.";

            await _emailService.SendEmailAsync(
                user?.Email
                ?? throw new BadRequestException("Email not found to be sent"),
                emailSubject,
                emailBody);
        }
    }
}
