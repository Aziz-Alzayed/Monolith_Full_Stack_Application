using FSTD.DataCore.Models.AuthModels;
using FSTD.DataCore.Models.JwtModels;
using FSTD.DataCore.Models.Users;

namespace FSTD.Application.MediatoR.Accounts.Auth.Repos
{
    public interface IAuthCommandsRepo
    {
        public Task<LoginResponseModel> LoginAsync(ApplicationUser user, string userPassword);
        public void LogoutUser(string userName);
        public Task<JwtAuthModel> RefreshTokenAsync(string accessToke, string refreshToken);
    }
}
