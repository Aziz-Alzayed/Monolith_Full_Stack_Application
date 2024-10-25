using FSTD.Application.MediatoR.Accounts.Users.Services;
using FSTD.DataCore.Models.Users;
using FSTD.Infrastructure.CommonServices.EndPointsRegisterServices;
using FSTD.Infrastructure.EmailServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Authentication;
using System.Web;

namespace FSTD.Infrastructure.MediatoR.Accounts.Users.Services
{
    [AutoRegister(ServiceLifetime.Scoped)]
    public class UsersService : IUsersService
    {
        UserManager<ApplicationUser> _userManager;
        IEmailService _emailService;

        public UsersService(UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task SendVerficationTokenAsync(string baseUrl, ApplicationUser newUser)
        {
            try
            {
                // Generate verification token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                // Create verification URL
                var userId = newUser.Id;
                var encodedToken = HttpUtility.UrlEncode(token);
                var verificationUrl = $"{baseUrl}/?userId={userId}&token={encodedToken}";

                await _emailService.SendEmailAsync(newUser.Email, "Verify Your Email", $"Please verify your email by clicking <a href='{verificationUrl}'>here</a>.");
            }
            catch
            {

                throw;
            }
        }
        public void ValidateSentFromEmail(string oldEmail)
        {
            if (string.IsNullOrEmpty(oldEmail))
            {
                throw new AuthenticationException("Authentication failed. Email could not be verified.");
            }
        }
    }
}
