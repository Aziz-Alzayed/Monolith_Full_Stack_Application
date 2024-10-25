using FSTD.Application.MediatoR.Project.Repos;
using FSTD.DataCore.Models;
using FSTD.Infrastructure.CommonServices.EndPointsRegisterServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FSTD.Infrastructure.MediatoR.Project.Repos
{
    [AutoRegister(ServiceLifetime.Scoped)]
    public class SeedCommandsRepo : ISeedCommandsRepo
    {
        private readonly ApplicationDbContext _context;

        public SeedCommandsRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteAllUsersAync()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var users = await _context.Users.ToListAsync();

                if (users.Any())
                {
                    // Delete all users
                    _context.Users.RemoveRange(users);

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
