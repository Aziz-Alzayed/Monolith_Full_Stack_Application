using AutoMapper;
using FSTD.Application.DTOs.Productivity.Tasks;
using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.Application.MediatoR.Productivity.Tasks.Commands;
using FSTD.Application.MediatoR.Productivity.Tasks.Repos;
using FSTD.DataCore.Models.ProductivityModels;
using FSTD.DataCore.Models.Users;
using FSTD.Infrastructure.MediatoR.Productivity.Tasks.Commands;
using Moq;

namespace FSTD.Infrastructure.Unit.Tests.MediatoR.Productivity.Tasks.Commands
{
    public class AddTaskCommandHandlerTests
    {
        private readonly Mock<ITaskCommandsRepo> _taskCommandsRepoMock;
        private readonly Mock<IUserQueriesRepo> _userQueriesRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AddTaskCommandHandler _handler;

        public AddTaskCommandHandlerTests()
        {
            _taskCommandsRepoMock = new Mock<ITaskCommandsRepo>();
            _userQueriesRepoMock = new Mock<IUserQueriesRepo>();
            _mapperMock = new Mock<IMapper>();

            _handler = new AddTaskCommandHandler(_taskCommandsRepoMock.Object, _userQueriesRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Add_Task_When_User_Is_Found()
        {
            // Arrange
            var user = new ApplicationUser { Id = Guid.NewGuid(), Email = "user@test.com" };
            var taskModel = new TasksModel { Name = "Test Task", Description = "Test Description" };
            var taskDto = new TaskDto { Name = "Test Task", Description = "Test Description" };

            var command = new AddTaskCommand(new AddTaskDto
            {
                Name = "Test Task",
                Description = "Test Description",
                IsDone = false,
                ValidUntil = DateTime.UtcNow.AddDays(1)
            }, user.Email);

            // Mock GetUserByEmailAsync to return the user
            _userQueriesRepoMock.Setup(repo => repo.GetUserByEmailAsync(user.Email)).ReturnsAsync(user);

            // Mock AddTaskAsync to return a task model
            _taskCommandsRepoMock.Setup(repo => repo.AddTaskAsync(It.IsAny<TasksModel>())).ReturnsAsync(taskModel);

            // Mock mapping from TasksModel to TaskDto
            _mapperMock.Setup(m => m.Map<TaskDto>(It.IsAny<TasksModel>())).Returns(taskDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(taskDto, result);
            _userQueriesRepoMock.Verify(repo => repo.GetUserByEmailAsync(user.Email), Times.Once);
            _taskCommandsRepoMock.Verify(repo => repo.AddTaskAsync(It.IsAny<TasksModel>()), Times.Once);
            _mapperMock.Verify(m => m.Map<TaskDto>(taskModel), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_UnauthorizedAccessException_When_User_Not_Found()
        {
            // Arrange
            var command = new AddTaskCommand(new AddTaskDto
            {
                Name = "Test Task",
                Description = "Test Description",
                IsDone = false,
                ValidUntil = DateTime.UtcNow.AddDays(1)
            }, "nonexistentuser@test.com");

            // Mock GetUserByEmailAsync to return null
            _userQueriesRepoMock.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));

            _userQueriesRepoMock.Verify(repo => repo.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
            _taskCommandsRepoMock.Verify(repo => repo.AddTaskAsync(It.IsAny<TasksModel>()), Times.Never);
            _mapperMock.Verify(m => m.Map<TaskDto>(It.IsAny<TasksModel>()), Times.Never);
        }
    }
}
