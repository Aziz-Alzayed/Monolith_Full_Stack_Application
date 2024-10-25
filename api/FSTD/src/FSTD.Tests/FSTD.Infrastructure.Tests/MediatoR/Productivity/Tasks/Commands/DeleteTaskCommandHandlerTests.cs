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
    public class DeleteTaskCommandHandlerTests
    {
        private readonly Mock<ITaskQueriesRepo> _taskQueriesRepoMock;
        private readonly Mock<ITaskCommandsRepo> _taskCommandsRepoMock;
        private readonly Mock<IUserQueriesRepo> _userQueriesRepoMock;
        private readonly DeleteTaskCommandHandler _handler;

        public DeleteTaskCommandHandlerTests()
        {
            _taskQueriesRepoMock = new Mock<ITaskQueriesRepo>();
            _taskCommandsRepoMock = new Mock<ITaskCommandsRepo>();
            _userQueriesRepoMock = new Mock<IUserQueriesRepo>();

            _handler = new DeleteTaskCommandHandler(
                _taskQueriesRepoMock.Object,
                _taskCommandsRepoMock.Object,
                _userQueriesRepoMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Delete_Task_When_User_Owns_Task()
        {
            // Arrange
            var task = new TasksModel { Id = Guid.NewGuid(), ApplicationUserId = Guid.NewGuid() };
            var user = new ApplicationUser { Id = task.ApplicationUserId, Email = "user@test.com" };

            var command = new DeleteTaskCommand(user.Email, task.Id);

            // Mocking the task retrieval and user retrieval
            _taskQueriesRepoMock.Setup(repo => repo.GetTaskByIdAsync(task.Id)).ReturnsAsync(task);
            _userQueriesRepoMock.Setup(repo => repo.GetUserByEmailAsync(user.Email)).ReturnsAsync(user);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _taskCommandsRepoMock.Verify(repo => repo.DeleteTaskAsync(task.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_Task_Is_Not_Found()
        {
            // Arrange
            var command = new DeleteTaskCommand("user@test.com", Guid.NewGuid());

            // Mock task not found
            _taskQueriesRepoMock.Setup(repo => repo.GetTaskByIdAsync(It.IsAny<Guid>())).ReturnsAsync((TasksModel)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_User_Is_Not_Found()
        {
            // Arrange
            var task = new TasksModel { Id = Guid.NewGuid(), ApplicationUserId = Guid.NewGuid() };
            var command = new DeleteTaskCommand("nonexistentuser@test.com", task.Id);

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

            var command = new DeleteTaskCommand(user.Email, task.Id);

            // Mock task and user retrieval
            _taskQueriesRepoMock.Setup(repo => repo.GetTaskByIdAsync(task.Id)).ReturnsAsync(task);
            _userQueriesRepoMock.Setup(repo => repo.GetUserByEmailAsync(user.Email)).ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
