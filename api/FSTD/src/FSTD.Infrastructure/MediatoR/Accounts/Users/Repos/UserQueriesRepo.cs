using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.DataCore.Models.Users;
using FSTD.Infrastructure.CommonServices.EndPointsRegisterServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FSTD.Infrastructure.MediatoR.Accounts.Users.Repos
{
    [AutoRegister(ServiceLifetime.Scoped)]
    public class UserQueriesRepo : IUserQueriesRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserQueriesRepo(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ApplicationUser?> GetUserByEmailAsync(string userEmail)
        {
            try
            {
                return await _userManager.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == userEmail.ToLower());
            }
            catch
            {
                throw;
            }
        }

        public async Task<ApplicationUser?> GetUserById(Guid id)
        {
            try
            {
                return await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch
            {
                throw;
            }
        }
    }
}
