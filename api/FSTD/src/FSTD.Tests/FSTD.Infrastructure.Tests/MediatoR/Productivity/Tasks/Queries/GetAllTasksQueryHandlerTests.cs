using AutoMapper;
using FSTD.Application.DTOs.Productivity.Tasks;
using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.Application.MediatoR.Productivity.Tasks.Queries;
using FSTD.Application.MediatoR.Productivity.Tasks.Repos;
using FSTD.DataCore.Models.ProductivityModels;
using FSTD.DataCore.Models.Users;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using FSTD.Infrastructure.MediatoR.Productivity.Tasks.Queries;
using Moq;

namespace FSTD.Infrastructure.Unit.Tests.MediatoR.Productivity.Tasks.Queries
{
    public class GetAllTasksQueryHandlerTests
    {
        private readonly Mock<ITaskQueriesRepo> _taskQueriesRepoMock;
        private readonly Mock<IUserQueriesRepo> _userQueriesRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllTasksQueryHandler _handler;

        public GetAllTasksQueryHandlerTests()
        {
            _taskQueriesRepoMock = new Mock<ITaskQueriesRepo>();
            _userQueriesRepoMock = new Mock<IUserQueriesRepo>();
            _mapperMock = new Mock<IMapper>();

            _handler = new GetAllTasksQueryHandler(
                _taskQueriesRepoMock.Object,
                _userQueriesRepoMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_List_Of_Tasks_When_User_Found()
        {
            // Arrange
            var user = new ApplicationUser { Id = Guid.NewGuid(), Email = "user@test.com" };
            var tasks = new List<TasksModel>
        {
            new TasksModel { Id = Guid.NewGuid(), Name = "Task 1", Description = "Task 1 description" },
            new TasksModel { Id = Guid.NewGuid(), Name = "Task 2", Description = "Task 2 description" }
        };
            var taskDtos = new List<TaskDto>
        {
            new TaskDto { Id = Guid.NewGuid(), Name = "Task 1", Description = "Task 1 description" },
            new TaskDto { Id = Guid.NewGuid(), Name = "Task 2", Description = "Task 2 description" }
        };

            var query = new GetAllTasksQuery(user.Email);

            // Mock the user retrieval and task retrieval
            _userQueriesRepoMock.Setup(repo => repo.GetUserByEmailAsync(user.Email)).ReturnsAsync(user);
            _taskQueriesRepoMock.Setup(repo => repo.GetAllTasksByUserIdAsync(user.Id)).ReturnsAsync(tasks);
            _mapperMock.Setup(m => m.Map<List<TaskDto>>(tasks)).Returns(taskDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(taskDtos, result);
            _userQueriesRepoMock.Verify(repo => repo.GetUserByEmailAsync(user.Email), Times.Once);
            _taskQueriesRepoMock.Verify(repo => repo.GetAllTasksByUserIdAsync(user.Id), Times.Once);
            _mapperMock.Verify(m => m.Map<List<TaskDto>>(tasks), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_User_Not_Found()
        {
            // Arrange
            var query = new GetAllTasksQuery("nonexistentuser@test.com");

            // Mock user not found
            _userQueriesRepoMock.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));

            _userQueriesRepoMock.Verify(repo => repo.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
            _taskQueriesRepoMock.Verify(repo => repo.GetAllTasksByUserIdAsync(It.IsAny<Guid>()), Times.Never);
            _mapperMock.Verify(m => m.Map<List<TaskDto>>(It.IsAny<List<TasksModel>>()), Times.Never);
        }
    }
}
