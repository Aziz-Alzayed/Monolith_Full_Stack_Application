using FSTD.Application.Models.Admin;
using FSTD.DataCore.Models.Users;

namespace FSTD.Application.MediatoR.Accounts.Admins.Repos
{
    public interface IAdminQueriesRepo
    {
        public Task<List<UserWithRolesModel>> GetAllApplicationUsersWithRolesAsync();
        public Task<UserWithRolesModel> GetlApplicationUserWithRolesAsync(ApplicationUser user);
    }
}
