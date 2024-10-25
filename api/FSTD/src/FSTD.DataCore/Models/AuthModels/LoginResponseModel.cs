using FSTD.DataCore.Models.JwtModels;
using FSTD.DataCore.Models.Users;

namespace FSTD.DataCore.Models.AuthModels
{
    public class LoginResponseModel
    {
        public ApplicationUser User { get; set; }
        public JwtAuthModel JwtAuth { get; set; }
    }
}
