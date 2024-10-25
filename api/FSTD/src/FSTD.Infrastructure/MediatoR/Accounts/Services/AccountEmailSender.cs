using FSTD.DataCore.Models.Users;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using FSTD.Infrastructure.CommonServices.EndPointsRegisterServices;
using FSTD.Infrastructure.EmailServices;
using Microsoft.Extensions.DependencyInjection;
using System.Web;

namespace FSTD.Infrastructure.MediatoR.Accounts.Services
{
    public interface IAccountEmailSender
    {
        Task SendPasswordResetEmailAsync(string resetPasswordUrl, string token, ApplicationUser user);
        Task SendVerificationEmailAsync(string resetPasswordUrl, string token, ApplicationUser user);
    }
    [AutoRegister(ServiceLifetime.Singleton)]
    public class AccountEmailSender : IAccountEmailSender
    {
        IEmailService _emailService;

        public AccountEmailSender(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task SendPasswordResetEmailAsync(string resetPasswordUrl, string token, ApplicationUser user)
        {
            try
            {
                // URL Encode the token and email
                var encodedToken = HttpUtility.UrlEncode(token);
                var encodedEmail = HttpUtility.UrlEncode(user.Email);

                // Construct the reset link to be sent via email
                var resetLink = $"{resetPasswordUrl}/?token={encodedToken}&email={encodedEmail}";

                // Prepare the email content
                var emailSubject = "Set Up Your Password";
                var emailBody = $"Welcome {user.FirstName}, please set up your password by clicking <a href='{resetLink}'>here</a>.";

                // Send the email
                await _emailService.SendEmailAsync(
                    user?.Email ??
                    throw new BadRequestException("Email not been found to send an email"),
                    emailSubject,
                    emailBody);
            }
            catch
            {

                throw;
            }
        }

        public async Task SendVerificationEmailAsync(string verificationUrl, string token, ApplicationUser user)
        {
            try
            {
                // URL Encode the token and email
                var encodedToken = HttpUtility.UrlEncode(token);
                var encodedUserId = HttpUtility.UrlEncode(user.Id.ToString());

                // Construct the reset link to be sent via email
                var resetLink = $"{verificationUrl}/?token={encodedToken}&userId={encodedUserId}";

                // Prepare the email content
                var emailSubject = "Verification Email";
                var emailBody = $"Welcome {user.FirstName}, please Verify your email by clicking <a href='{resetLink}'>here</a>.";

                // Send the email
                await _emailService.SendEmailAsync(
                    user?.Email ??
                    throw new BadRequestException("Email not been found to send an email"),
                    emailSubject,
                    emailBody);
            }
            catch
            {

                throw;
            }
        }
    }
}
