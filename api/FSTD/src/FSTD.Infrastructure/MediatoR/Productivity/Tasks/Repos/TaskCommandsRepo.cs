using FSTD.Application.MediatoR.Productivity.Tasks.Repos;
using FSTD.DataCore.Models;
using FSTD.DataCore.Models.ProductivityModels;
using FSTD.Infrastructure.CommonServices.EndPointsRegisterServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FSTD.Infrastructure.MediatoR.Productivity.Tasks.Repos
{
    [AutoRegister(ServiceLifetime.Scoped)]
    public class TaskCommandsRepo(
        ApplicationDbContext _dbContext
        ) : ITaskCommandsRepo
    {
        public async Task<TasksModel> AddTaskAsync(TasksModel tasksModel)
        {
            try
            {
                await _dbContext.Tasks.AddAsync(tasksModel);
                await _dbContext.SaveChangesAsync();

                return tasksModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteTaskAsync(Guid id)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(); // Begin transaction
            try
            {
                var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
                if (task == null)
                {
                    throw new KeyNotFoundException($"Task with Id {id} not found.");
                }

                _dbContext.Tasks.Remove(task);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateTaskAsync(TasksModel tasksModel)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                _dbContext.Tasks.Update(tasksModel);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
