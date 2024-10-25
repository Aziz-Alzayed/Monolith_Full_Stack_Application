using FSTD.DataCore.Authentication;
using FSTD.DataCore.Models.Users;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using FSTD.Infrastructure.CommonServices.EndPointsRegisterServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FSTD.Infrastructure.MediatoR.Accounts.Services
{
    public interface IAccountService
    {
        Task<ApplicationUser> GetUserAsync(Guid userId);
        Task<ApplicationUser> GetUserByEmailAsync(string userEmail);
        Task<List<string>> GetUserRolesAsync(ApplicationUser user);
        Task<bool> IsAdminUserAsync(ApplicationUser user);
        Task<bool> IsSuperUserAsync(ApplicationUser user);
        Task<bool> IsAdminUserByEmailAsync(string userEmail);
        Task<bool> IsSuperUserByEmailAsync(string userEmail);
        Task<bool> CheckPermissionsIsAdminOrSuperAsync(string userEmail);
        Task<bool> CanAssignRolesAsync(string userEmail, string[] requestedRoles);
    }
    [AutoRegister(ServiceLifetime.Scoped)]
    public class AccountService : IAccountService
    {
        UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetUserAsync(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    throw new NotFoundException("User with the given Id not exists!");
                }
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string userEmail)
        {
            try
            {
                if (userEmail == string.Empty)
                    throw new NotFoundException("userEmail is empty");

                var user = await _userManager.FindByEmailAsync(userEmail);
                if (user == null)
                {
                    throw new NotFoundException("User with the given email not exists!");
                }
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<string>> GetUserRolesAsync(ApplicationUser user)
        {
            try
            {
                var roles = await _userManager.GetRolesAsync(user);
                return roles.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> IsAdminUserAsync(ApplicationUser user)
        {
            try
            {
                var roles = await GetUserRolesAsync(user);
                return roles.Contains(AppRoles.Admin);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> IsAdminUserByEmailAsync(string userEmail)
        {
            try
            {
                if (userEmail == string.Empty)
                    throw new NotFoundException("userEmail is empty");

                var user = await GetUserByEmailAsync(userEmail);
                if (user == null)
                {
                    throw new UnauthorizedAccessException();
                }
                return await IsAdminUserAsync(user);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> IsSuperUserAsync(ApplicationUser user)
        {
            try
            {
                var roles = await GetUserRolesAsync(user);
                return roles.Contains(AppRoles.Super);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> IsSuperUserByEmailAsync(string userEmail)
        {
            try
            {
                if (userEmail == string.Empty)
                    throw new NotFoundException("userEmail is empty");

                var user = await GetUserByEmailAsync(userEmail);

                return await IsSuperUserAsync(user);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> CheckPermissionsIsAdminOrSuperAsync(string userEmail)
        {
            try
            {
                if (userEmail == string.Empty)
                    throw new NotFoundException("userEmail is empty");

                var user = await GetUserByEmailAsync(userEmail);
                if (user == null)
                {
                    throw new UnauthorizedAccessException();
                }
                var isAdmin = await IsAdminUserAsync(user);
                var Super = await IsSuperUserAsync(user);
                return isAdmin || Super;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> CanAssignRolesAsync(string userEmail, string[] requestedRoles)
        {
            try
            {
                var user = await GetUserByEmailAsync(userEmail);
                if (user == null)
                {
                    throw new NotFoundException("User with the given email not exists!");
                }

                var userRoles = await GetUserRolesAsync(user);
                bool isSuper = userRoles.Contains(AppRoles.Super);
                bool isAdmin = userRoles.Contains(AppRoles.Admin);

                // If the user is Super, they can assign any roles
                if (isSuper)
                {
                    return true;
                }

                // Checking if the requestedRoles contains Super and if the user is not a Super
                if (requestedRoles.Contains(AppRoles.Super) && !isSuper)
                {
                    return false; // Only Super can assign Super
                }

                // If the user is Admin, they can only assign Admin and User roles, and cannot assign Super
                if (isAdmin &&
                    requestedRoles.All(role =>
                    string.Equals(role, AppRoles.Admin, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(role, AppRoles.User, StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }

                // If reaching here, it means either a User trying to assign roles or an Admin trying to assign Super, both are not allowed
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
