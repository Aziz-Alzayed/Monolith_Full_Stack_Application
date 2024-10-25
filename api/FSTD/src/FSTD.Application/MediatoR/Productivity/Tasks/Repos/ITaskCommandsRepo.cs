using FSTD.DataCore.Models.ProductivityModels;

namespace FSTD.Application.MediatoR.Productivity.Tasks.Repos
{
    public interface ITaskCommandsRepo
    {
        Task UpdateTaskAsync(TasksModel tasksModel);
        Task<TasksModel> AddTaskAsync(TasksModel tasksModel);
        Task DeleteTaskAsync(Guid id);
    }
}
