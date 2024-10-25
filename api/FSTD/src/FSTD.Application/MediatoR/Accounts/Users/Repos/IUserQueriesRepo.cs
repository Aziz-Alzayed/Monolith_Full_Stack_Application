using FSTD.DataCore.Models.Users;

namespace FSTD.Application.MediatoR.Accounts.Users.Repos
{
    public interface IUserQueriesRepo
    {
        Task<ApplicationUser?> GetUserByEmailAsync(string userEmail);
        Task<ApplicationUser?> GetUserById(Guid id);
    }
}
