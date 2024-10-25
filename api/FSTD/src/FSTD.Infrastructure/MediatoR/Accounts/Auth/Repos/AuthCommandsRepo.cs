using FSTD.Application.MediatoR.Accounts.Auth.Repos;
using FSTD.Application.MediatoR.Accounts.Auth.Services;
using FSTD.DataCore.Models.AuthModels;
using FSTD.DataCore.Models.JwtModels;
using FSTD.DataCore.Models.Users;
using FSTD.Infrastructure.CommonServices.EndPointsRegisterServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace FSTD.Infrastructure.MediatoR.Accounts.Auth.Repos
{
    [AutoRegister(ServiceLifetime.Scoped)]
    public class AuthCommandsRepo : IAuthCommandsRepo
    {
        UserManager<ApplicationUser> userManager;
        SignInManager<ApplicationUser> signInManager;
        IJwtService jwtService;

        public AuthCommandsRepo(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtService jwtService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtService = jwtService;
        }

        public async Task<LoginResponseModel> LoginAsync(ApplicationUser user, string userPassword)
        {
            try
            {
                //if (!user.EmailConfirmed)
                //{
                //    throw new InvalidOperationException("Email not verified.");
                //}

                var result = await signInManager.CheckPasswordSignInAsync(user, userPassword, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    // Create claims and generate JWT
                    var userRoles = await userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, user.Email)
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var jwtResult = jwtService.GenerateTokens(user.Email, authClaims.ToArray(), DateTime.Now);

                    // Sign in the user
                    await signInManager.SignInAsync(user, false);

                    return new LoginResponseModel
                    {
                        User = user,
                        JwtAuth = jwtResult,
                    };
                }
                else if (result.IsLockedOut)
                {
                    throw new InvalidOperationException("User account is locked.");
                }
                else
                {
                    await userManager.AccessFailedAsync(user);
                    throw new InvalidOperationException("Invalid login attempt.");
                }
            }
            catch
            {
                throw;
            }
        }

        public void LogoutUser(string userEmail)
        {
            try
            {
                jwtService.RemoveRefreshTokenByUserName(userEmail);
            }
            catch
            {

                throw;
            }
        }

        public async Task<JwtAuthModel> RefreshTokenAsync(string accessToke, string refreshToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    throw new UnauthorizedAccessException("Refresh token is required.");
                }

                return jwtService.Refresh(refreshToken, accessToke, DateTime.Now);

            }
            catch
            {
                throw;
            }
        }
    }
}
