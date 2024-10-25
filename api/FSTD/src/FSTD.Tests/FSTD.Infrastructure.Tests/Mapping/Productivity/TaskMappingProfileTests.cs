using AutoMapper;
using FSTD.Application.DTOs.Productivity.Tasks;
using FSTD.DataCore.Models.ProductivityModels;
using FSTD.DataCore.Models.Users;
using FSTD.Infrastructure.Mapping.Productivity;

namespace FSTD.Infrastructure.Unit.Tests.Mapping.Productivity
{
    public class TaskMappingProfileTests
    {
        private readonly IMapper _mapper;

        public TaskMappingProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TaskMappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void TaskModel_To_TaskDto_Mapping_Is_Valid()
        {
            // Arrange
            var taskModel = new TasksModel
            {
                Id = Guid.NewGuid(),
                Name = "Test Task",
                Description = "This is a test task description.",
                IsDone = false,
                ValidUntil = DateTime.Now.AddDays(5),
                ApplicationUserId = Guid.NewGuid(),
                ApplicationUser = new ApplicationUser { FirstName = "John", LastName = "Doe" }
            };

            // Act
            var taskDto = _mapper.Map<TaskDto>(taskModel);

            // Assert
            Assert.Equal(taskModel.Id, taskDto.Id);
            Assert.Equal(taskModel.Name, taskDto.Name);
            Assert.Equal(taskModel.Description, taskDto.Description);
            Assert.Equal(taskModel.IsDone, taskDto.IsDone);
            Assert.Equal(taskModel.ValidUntil, taskDto.ValidUntil);
        }

        [Fact]
        public void TaskDto_To_TaskModel_Mapping_Is_Valid()
        {
            // Arrange
            var taskDto = new TaskDto
            {
                Id = Guid.NewGuid(),
                Name = "Test Task",
                Description = "This is a test task description.",
                IsDone = false,
                ValidUntil = DateTime.Now.AddDays(5)
            };

            // Act
            var taskModel = _mapper.Map<TasksModel>(taskDto);

            // Assert
            Assert.Equal(taskDto.Id, taskModel.Id);
            Assert.Equal(taskDto.Name, taskModel.Name);
            Assert.Equal(taskDto.Description, taskModel.Description);
            Assert.Equal(taskDto.IsDone, taskModel.IsDone);
            Assert.Equal(taskDto.ValidUntil, taskModel.ValidUntil);
        }
    }
}
