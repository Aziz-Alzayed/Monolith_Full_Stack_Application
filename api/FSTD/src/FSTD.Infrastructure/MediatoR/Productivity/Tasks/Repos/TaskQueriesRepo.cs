using FSTD.Application.MediatoR.Productivity.Tasks.Repos;
using FSTD.DataCore.Models;
using FSTD.DataCore.Models.ProductivityModels;
using FSTD.Infrastructure.CommonServices.EndPointsRegisterServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FSTD.Infrastructure.MediatoR.Productivity.Tasks.Repos
{
    [AutoRegister(ServiceLifetime.Scoped)]
    internal class TaskQueriesRepo(
        ApplicationDbContext _dbContext
        ) : ITaskQueriesRepo
    {
        public async Task<List<TasksModel>> GetAllTasksByUserIdAsync(Guid userId)
        {
            try
            {
                return await _dbContext.Tasks
                    .Where(task => task.ApplicationUserId == userId)
                    .ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<TasksModel> GetTaskByIdAsync(Guid id)
        {
            try
            {
                return await _dbContext.Tasks
                    .FirstOrDefaultAsync(task => task.Id == id);
            }
            catch
            {
                throw;
            }
        }
    }
}
