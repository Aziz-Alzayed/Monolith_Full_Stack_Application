using FSTD.DataCore.Models.Users;

namespace FSTD.Application.MediatoR.Accounts.Users.Services
{
    public interface IUsersService
    {
        public Task SendVerficationTokenAsync(string baseUrl, ApplicationUser newUser);
        public void ValidateSentFromEmail(string oldEmail);
    }
}
