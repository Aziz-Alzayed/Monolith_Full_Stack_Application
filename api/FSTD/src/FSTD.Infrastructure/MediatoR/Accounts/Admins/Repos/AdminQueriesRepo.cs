using FSTD.Application.MediatoR.Accounts.Admins.Repos;
using FSTD.Application.Models.Admin;
using FSTD.DataCore.Models.Users;
using FSTD.Infrastructure.CommonServices.EndPointsRegisterServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FSTD.Infrastructure.MediatoR.Accounts.Admins.Repos
{
    [AutoRegister(ServiceLifetime.Scoped)]
    public class AdminQueriesRepo : IAdminQueriesRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminQueriesRepo(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserWithRolesModel>> GetAllApplicationUsersWithRolesAsync()
        {
            try
            {
                var usersWithRoles = new List<UserWithRolesModel>();

                var users = await _userManager.Users.ToListAsync();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    usersWithRoles.Add(new UserWithRolesModel
                    {
                        User = user,
                        Roles = roles.ToList()
                    });
                }

                return usersWithRoles;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserWithRolesModel> GetlApplicationUserWithRolesAsync(ApplicationUser user)
        {
            try
            {
                var roles = await _userManager.GetRolesAsync(user) ?? new List<string>(); ;
                var userWithRolws = new UserWithRolesModel
                {
                    User = user,
                    Roles = roles.ToList()
                };
                return userWithRolws;
            }
            catch
            {
                throw;
            }
        }
    }
}
