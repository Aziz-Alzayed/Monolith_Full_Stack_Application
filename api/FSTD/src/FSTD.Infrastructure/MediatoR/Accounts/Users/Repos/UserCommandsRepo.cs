using FSTD.Application.MediatoR.Accounts.Auth.Repos;
using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.Application.MediatoR.Accounts.Users.Services;
using FSTD.DataCore.Authentication;
using FSTD.DataCore.Models.Users;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using FSTD.Infrastructure.CommonServices.EndPointsRegisterServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FSTD.Infrastructure.MediatoR.Accounts.Users.Repos
{
    [AutoRegister(ServiceLifetime.Scoped)]
    public class UserCommandsRepo : IUserCommandsRepo
    {
        UserManager<ApplicationUser> _userManager;
        IAuthCommandsRepo _authRepository;
        IUsersService _userService;

        public UserCommandsRepo(UserManager<ApplicationUser> userManager, IAuthCommandsRepo authRepository, IUsersService userService)
        {
            _userManager = userManager;
            _authRepository = authRepository;
            _userService = userService;
        }

        public async Task DeleteUserAsync(string userEmail)
        {
            try
            {
                _userService.ValidateSentFromEmail(userEmail);

                var user = await _userManager.FindByEmailAsync(userEmail);
                if (user == null)
                { throw new NotFoundException("User has not been found!"); }
                _authRepository.LogoutUser(user.Email);

                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                    throw new Exception("Cannot delete this user!");
            }
            catch
            {
                throw;
            }
        }
        public async Task RegisterUserAsync(string firstName, string lastName, string email, string password, string baseUrl)
        {
            try
            {
                // Check if a user with the given email already exists
                var existingUser = await _userManager.FindByEmailAsync(email);
                if (existingUser != null)
                {
                    throw new ConflictException("User with the given email already exists.");
                }

                // Create a new user object
                var newUser = new ApplicationUser
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    EmailConfirmed = false,
                    CreatedOn = DateTime.UtcNow
                };

                // Create the user with the password
                var createUserResult = await _userManager.CreateAsync(newUser, password);
                if (!createUserResult.Succeeded)
                {
                    throw new InvalidOperationException("User could not be created.");
                }

                // Add User to Role
                var addToRoleResult = await _userManager.AddToRoleAsync(newUser, AppRoles.User);
                if (!addToRoleResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to add user to role.");
                }

                // Send verification token
                await _userService.SendVerficationTokenAsync(baseUrl, newUser);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UpdateUserPasswordAsync(ApplicationUser user, string oldPassowrd, string newPassword)
        {
            try
            {
                // Check the old password
                var checkOldPassword = await _userManager.CheckPasswordAsync(user, oldPassowrd);
                if (!checkOldPassword)
                {
                    throw new ArgumentException("Old password is incorrect.");
                }

                // Update the password
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, oldPassowrd, newPassword);
                if (!changePasswordResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to update the password.");
                }
            }
            catch
            {
                throw;
            }
        }
        public async Task VerifyUserEmailAsync(ApplicationUser user, string verficationToken)
        {
            try
            {
                var result = await _userManager.ConfirmEmailAsync(user, verficationToken);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Email verification failed.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task ResetUserPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            try
            {
                // Attempt to reset the user's password
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (!result.Succeeded)
                {
                    // Handle the failure scenario, possibly logging the errors or throwing an exception
                    throw new InvalidOperationException("Password reset failed.");
                }
            }
            catch
            {

                throw;
            }
        }
        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser applicationUser)
        {
            try
            {
                return await _userManager.GeneratePasswordResetTokenAsync(applicationUser);
            }
            catch
            {

                throw;
            }
        }
        public async Task UpdateUserAsync(ApplicationUser applicationUser)
        {
            try
            {
                var result = await _userManager.UpdateAsync(applicationUser);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("User has not been updated!");
                }
            }
            catch
            {

                throw;
            }
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser applicationUser)
        {
            try
            {
                return await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
            }
            catch
            {

                throw;
            }
        }
    }
}

