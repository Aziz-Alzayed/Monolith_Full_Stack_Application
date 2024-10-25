using FSTD.DataCore.Models.Users;

namespace FSTD.Application.Models.Admin
{
    public class UserWithRolesModel
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}
