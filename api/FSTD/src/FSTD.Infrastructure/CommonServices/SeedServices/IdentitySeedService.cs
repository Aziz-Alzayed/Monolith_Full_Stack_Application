using FSTD.DataCore.Authentication;
using FSTD.DataCore.Models;
using FSTD.DataCore.Models.ProductivityModels; // Import TaskModel
using FSTD.DataCore.Models.Users;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using FSTD.Infrastructure.CommonServices.EndPointsRegisterServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FSTD.Infrastructure.CommonServices.SeedServices
{
    public interface IIdentitySeedService
    {
        Task SeedRoles();
        Task SeedSuperUser();
        Task SeedAdminUser();
        Task SeedUser();
    }

    [AutoRegister(ServiceLifetime.Scoped)]
    public class IdentitySeedService : IIdentitySeedService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly ApplicationDbContext _context; // Inject your DbContext to handle tasks

        public IdentitySeedService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            ApplicationDbContext context) // Inject DbContext for task seeding
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task SeedRoles()
        {
            var roles = new List<string> { AppRoles.Super, AppRoles.Admin, AppRoles.User };
            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }
        }

        public async Task SeedAdminUser()
        {
            var adminUser = new ApplicationUser
            {
                UserName = ("admin@example.com").ToUpper(),
                Email = "admin@example.com",
                FirstName = AppRoles.Admin,
                LastName = AppRoles.Admin,
                EmailConfirmed = true
            };

            await CreateUserWithTasks(adminUser, "AdminPassword123!", AppRoles.Admin);
        }

        public async Task SeedSuperUser()
        {
            var superUser = new ApplicationUser
            {
                UserName = ("superuser@example.com").ToUpper(),
                Email = "superuser@example.com",
                FirstName = AppRoles.Super,
                LastName = AppRoles.Super,
                EmailConfirmed = true
            };

            await CreateUserWithTasks(superUser, "SuperUserPassword123!", AppRoles.Super);
        }

        public async Task SeedUser()
        {
            var user = new ApplicationUser
            {
                UserName = ("user@example.com").ToUpper(),
                Email = "user@example.com",
                FirstName = AppRoles.User,
                LastName = AppRoles.User,
                EmailConfirmed = true
            };

            await CreateUserWithTasks(user, "UserPassword123!", AppRoles.User);
        }

        private async Task CreateUserWithTasks(ApplicationUser user, string password, string role)
        {
            var existUser = await _userManager.FindByEmailAsync(
                user?.Email ??
                throw new BadRequestException());

            if (existUser == null)
            {
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);

                    // Create tasks for the user
                    await CreateTasksForUser(user);
                }
            }
        }

        private async Task CreateTasksForUser(ApplicationUser user)
        {
            // Check if the user already has tasks (to avoid duplicates)
            if (!_context.Tasks.Any(t => t.ApplicationUserId == user.Id))
            {
                var tasks = new List<TasksModel>
                {
                    new TasksModel
                    {
                        Name = "Task 1 for " + user.FirstName,
                        Description = "Description of task 1",
                        IsDone = true,
                        ValidUntil = DateTime.UtcNow.AddDays(30),
                        ApplicationUserId = user.Id
                    },
                    new TasksModel
                    {
                        Name = "Task 2 for " + user.FirstName,
                        Description = "Description of task 2",
                        IsDone = false,
                        ValidUntil = DateTime.UtcNow.AddDays(10),
                        ApplicationUserId = user.Id
                    }
                };

                _context.Tasks.AddRange(tasks);
                await _context.SaveChangesAsync();
            }
        }
    }
}
