using FSTD.DataCore.Models.Users;

namespace FSTD.Application.MediatoR.Accounts.Users.Repos
{
    public interface IUserCommandsRepo
    {
        Task RegisterUserAsync(string firstName, string lastName, string email, string password, string baseUrl);
        Task DeleteUserAsync(string userEmail);
        Task UpdateUserPasswordAsync(ApplicationUser user, string oldPassowrd, string newPassword);
        Task UpdateUserAsync(ApplicationUser applicationUser);
        Task VerifyUserEmailAsync(ApplicationUser user, string verficationToken);
        Task ResetUserPasswordAsync(ApplicationUser user, string token, string newPassword);
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser applicationUser);
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser applicationUser);
    }
}
