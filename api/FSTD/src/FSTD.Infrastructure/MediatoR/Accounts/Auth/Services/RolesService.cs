using FSTD.DataCore.Authentication;
using FSTD.Infrastructure.CommonServices.EndPointsRegisterServices;
using Microsoft.Extensions.DependencyInjection;

namespace FSTD.Infrastructure.MediatoR.Accounts.Auth.Services
{
    public interface IRolesService
    {
        List<string> GetRolesList();
        List<string> ValidateRoles(List<string> requestedRoles);
    }
    [AutoRegister(ServiceLifetime.Singleton)]
    public class RolesService : IRolesService
    {
        public List<string> GetRolesList()
        {
            return new List<string>() { AppRoles.Super, AppRoles.Admin, AppRoles.User };
        }
        public List<string> ValidateRoles(List<string> requestedRoles)
        {
            var validRoles = GetRolesList().Select(role => role.ToLower()).ToList();

            // Filter out any roles not present in the validRoles list
            var validatedRoles = requestedRoles
            .Select(role => role.ToLower().Trim())
            .Where(role => validRoles.Contains(role))
            .Distinct()
            .ToList();

            // If any of the requested roles are not valid, return an empty list
            if (validatedRoles.Count != requestedRoles.Count)
            {
                return new List<string>();
            }

            return validatedRoles;
        }
    }
}
