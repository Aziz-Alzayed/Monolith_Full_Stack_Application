using FSTD.Application.MediatoR.Accounts.Admins.Repos;
using FSTD.Application.MediatoR.Accounts.Auth.Repos;
using FSTD.DataCore.Models.Users;
using FSTD.Infrastructure.CommonServices.EndPointsRegisterServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FSTD.Infrastructure.MediatoR.Accounts.Admins.Repos
{
    [AutoRegister(ServiceLifetime.Scoped)]
    public class AdminCommandsRepo : IAdminCommandsRepo
    {
        UserManager<ApplicationUser> userManager;
        IAuthCommandsRepo authRepository;
        RoleManager<IdentityRole<Guid>> roleManager;

        public AdminCommandsRepo(
            UserManager<ApplicationUser> userManager,
            IAuthCommandsRepo authRepository,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            this.userManager = userManager;
            this.authRepository = authRepository;
            this.roleManager = roleManager;
        }

        public async Task AddNewUserAsync(ApplicationUser user)
        {
            try
            {
                // Create the user with no password
                var result = await userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Could not create user.");
                }

            }
            catch
            {
                throw;
            }
        }

        public async Task AddRolesAsync(ApplicationUser user, List<string> roles)
        {
            try
            {
                // If roles are specified, add the user to these roles
                if (roles != null && roles.Count > 0)
                {
                    foreach (var role in roles)
                    {
                        var addToRoleResult = await userManager.AddToRoleAsync(user, role);
                        if (!addToRoleResult.Succeeded)
                        {
                            throw new InvalidOperationException($"Failed to add user to role {role}.");
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteUserByIdAsync(ApplicationUser user)
        {
            try
            {
                authRepository.LogoutUser(user.Email);

                var result = await userManager.DeleteAsync(user);

                if (!result.Succeeded)
                    throw new Exception("Cannot delete this user!");
            }
            catch
            {
                throw;
            }
        }

        public async Task UpdateRolesAsync(ApplicationUser user, List<string> roles)
        {
            try
            {
                // Convert roles to uppercase for consistent comparison
                var normalizedRoles = roles.Select(r => r.ToUpperInvariant()).ToList();

                // Get current roles of the user and normalize them
                var currentRoles = (await userManager.GetRolesAsync(user))
                    .Select(r => r.ToUpperInvariant())
                    .ToList();

                // Determine roles to add and remove using case-insensitive comparison
                var rolesToAdd = normalizedRoles.Except(currentRoles, StringComparer.OrdinalIgnoreCase);
                var rolesToRemove = currentRoles.Except(normalizedRoles, StringComparer.OrdinalIgnoreCase);

                // Add user to new roles
                foreach (var role in rolesToAdd)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        // Skip adding the user to a non-existent role
                        continue;
                    }
                    await userManager.AddToRoleAsync(user, role);
                }

                // Remove user from old roles
                foreach (var role in rolesToRemove)
                {
                    await userManager.RemoveFromRoleAsync(user, role);
                }
            }
            catch
            {
                throw;
            }
        }


        public async Task UpdateUserAsync(ApplicationUser user)
        {
            try
            {
                // Update user information
                var result = await userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Failed to update user.");
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
