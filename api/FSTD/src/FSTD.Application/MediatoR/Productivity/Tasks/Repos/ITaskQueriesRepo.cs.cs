using FSTD.DataCore.Models.ProductivityModels;

namespace FSTD.Application.MediatoR.Productivity.Tasks.Repos
{
    public interface ITaskQueriesRepo
    {
        Task<List<TasksModel>> GetAllTasksByUserIdAsync(Guid userId);
        Task<TasksModel> GetTaskByIdAsync(Guid id);
    }
}
