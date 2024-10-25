using FSTD.Application.DTOs.Accounts.Users;

namespace FSTD.Application.DTOs.Accounts.Auths
{
    public class AccessTokenDto
    {
        public UserInfoDto UserInfo { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
