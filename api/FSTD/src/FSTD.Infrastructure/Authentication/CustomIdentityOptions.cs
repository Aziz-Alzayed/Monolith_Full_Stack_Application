using FSTD.Application.Validations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FSTD.Infrastructure.Authentication
{
    public class CustomIdentityOptions
    {
        public static void ConfigureIdentityOptions(IServiceCollection services)
        {

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = PasswordRules.RequireDigit;
                options.Password.RequireLowercase = PasswordRules.RequireLowercase;
                options.Password.RequireNonAlphanumeric = PasswordRules.RequireNonAlphanumeric;
                options.Password.RequireUppercase = PasswordRules.RequireUppercase;
                options.Password.RequiredLength = PasswordRules.RequiredLength;
                options.Password.RequiredUniqueChars = PasswordRules.RequiredUniqueChars;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

        }
    }
}
