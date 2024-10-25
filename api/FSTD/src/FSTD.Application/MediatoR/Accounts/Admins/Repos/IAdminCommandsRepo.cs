using FSTD.DataCore.Models.Users;

namespace FSTD.Application.MediatoR.Accounts.Admins.Repos
{
    public interface IAdminCommandsRepo
    {
        Task AddNewUserAsync(ApplicationUser user);
        Task UpdateUserAsync(ApplicationUser user);
        Task AddRolesAsync(ApplicationUser user, List<string> roles);
        Task UpdateRolesAsync(ApplicationUser user, List<string> roles);
        Task DeleteUserByIdAsync(ApplicationUser user);
    }
}
