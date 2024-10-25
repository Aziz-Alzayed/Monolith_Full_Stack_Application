using FSTD.Application.DTOs.Productivity.Tasks;
using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.Application.MediatoR.Productivity.Tasks.Commands;
using FSTD.Application.MediatoR.Productivity.Tasks.Repos;
using FSTD.DataCore.Models.ProductivityModels;
using FSTD.DataCore.Models.Users;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using FSTD.Infrastructure.MediatoR.Productivity.Tasks.Commands;
using Moq;

namespace FSTD.Infrastructure.Unit.Tests.MediatoR.Productivity.Tasks.Commands
{
    public class UpdateTaskCommandHandlerTests
    {
        private readonly Mock<ITaskQueriesRepo> _taskQueriesRepoMock;
        private readonly Mock<ITaskCommandsRepo> _taskCommandsRepoMock;
        private readonly Mock<IUserQueriesRepo> _userQueriesRepoMock;
        private readonly UpdateTaskCommandHandler _handler;

        public UpdateTaskCommandHandlerTests()
        {
            _taskQueriesRepoMock = new Mock<ITaskQueriesRepo>();
            _taskCommandsRepoMock = new Mock<ITaskCommandsRepo>();
            _userQueriesRepoMock = new Mock<IUserQueriesRepo>();

            _handler = new UpdateTaskCommandHandler(
                _taskQueriesRepoMock.Object,
                _taskCommandsRepoMock.Object,
                _userQueriesRepoMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Update_Task_When_User_Owns_It()
        {
            // Arrange
            var task = new TasksModel { Id = Guid.NewGuid(), ApplicationUserId = Guid.NewGuid() };
            var user = new ApplicationUser { Id = task.ApplicationUserId, Email = "user@test.com" };

            var dto = new TaskDto
            {
                Id = task.Id,
                Name = "Updated Task",
                Description = "Updated Description",
                IsDone = true,
                ValidUntil = DateTime.UtcNow.AddDays(2)
            };

            var command = new UpdateTaskCommand(dto, user.Email);

            // Mocking task retrieval and user retrieval
            _taskQueriesRepoMock.Setup(repo => repo.GetTaskByIdAsync(dto.Id)).ReturnsAsync(task);
            _userQueriesRepoMock.Setup(repo => repo.GetUserByEmailAsync(user.Email)).ReturnsAsync(user);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _taskCommandsRepoMock.Verify(repo => repo.UpdateTaskAsync(It.IsAny<TasksModel>()), Times.Once);
            Assert.Equal("Updated Task", task.Name);
            Assert.Equal("Updated Description", task.Description);
            Assert.True(task.IsDone);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_Task_Is_Not_Found()
        {
            // Arrange
            var dto = new TaskDto { Id = Guid.NewGuid() };
            var command = new UpdateTaskCommand(dto, "user@test.com");

            // Mock task not found
            _taskQueriesRepoMock.Setup(repo => repo.GetTaskByIdAsync(dto.Id)).ReturnsAsync((TasksModel)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_User_Is_Not_Found()
        {
            // Arrange
            var task = new TasksModel { Id = Guid.NewGuid(), ApplicationUserId = Guid.NewGuid() };
            var dto = new TaskDto { Id = task.Id };
            var command = new UpdateTaskCommand(dto, "nonexistentuser@test.com");

            // Mock the task retrieval but user not found
            _taskQueriesRepoMock.Setup(repo => repo.GetTaskByIdAsync(task.Id)).ReturnsAsync(task);
            _userQueriesRepoMock.Setup(repo => repo.GetUserByEmailAsync(command.UserEmail)).ReturnsAsync((ApplicationUser)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_UnauthorizedAccessException_When_User_Does_Not_Own_Task()
        {
            // Arrange
            var task = new TasksModel { Id = Guid.NewGuid(), ApplicationUserId = Guid.NewGuid() };
            var user = new ApplicationUser { Id = Guid.NewGuid(), Email = "user@test.com" }; // Different user

            var dto = new TaskDto { Id = task.Id };
            var command = new UpdateTaskCommand(dto, user.Email);

            // Mock task and user retrieval
            _taskQueriesRepoMock.Setup(repo => repo.GetTaskByIdAsync(task.Id)).ReturnsAsync(task);
            _userQueriesRepoMock.Setup(repo => repo.GetUserByEmailAsync(user.Email)).ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
