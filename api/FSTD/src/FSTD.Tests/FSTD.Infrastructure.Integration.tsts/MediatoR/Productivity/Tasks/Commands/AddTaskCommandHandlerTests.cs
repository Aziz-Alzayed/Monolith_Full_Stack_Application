using FSTD.Application.DTOs.Productivity.Tasks;
using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.Application.MediatoR.Productivity.Tasks.Commands;
using FSTD.Application.MediatoR.Productivity.Tasks.Repos;
using FSTD.DataCore.Models;
using FSTD.DataCore.Models.Users;
using FSTD.Infrastructure.Mapping.Productivity;
using FSTD.Infrastructure.MediatoR.Productivity.Tasks.Commands;
using FSTD.Infrastructure.MediatoR.Productivity.Tasks.Repos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace FSTD.Infrastructure.Integration.Tests.MediatoR.Productivity.Tasks.Commands
{
    public class AddTaskCommandHandlerTests : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Mock<IUserQueriesRepo> _userQueriesRepoMock;
        private readonly ApplicationDbContext _dbContext;

        public AddTaskCommandHandlerTests()
        {
            // Set up the service collection
            var services = new ServiceCollection();

            // Set up InMemory Database for testing
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));

            // Set up AutoMapper
            services.AddAutoMapper(typeof(TaskMappingProfile));

            // Add the repositories to DI
            services.AddScoped<ITaskCommandsRepo, TaskCommandsRepo>();

            // Add MediatR and specify the assembly that contains the handlers
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddTaskCommandHandler).Assembly));

            // Add Mock for IUserQueriesRepo
            _userQueriesRepoMock = new Mock<IUserQueriesRepo>();
            services.AddSingleton(_userQueriesRepoMock.Object);

            // Build the service provider
            _serviceProvider = services.BuildServiceProvider();

            // Get the DbContext
            _dbContext = _serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Ensure the database is created before each test
            _dbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task Handle_AddsTaskSuccessfully_ReturnsTaskDto()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = "user@test.com",
                FirstName = "Test",
                LastName = "Test",
            };

            // Mock the user repository
            _userQueriesRepoMock.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            var addTaskDto = new AddTaskDto
            {
                Name = "Test Task",
                Description = "Test Task Description",
                IsDone = false,
                ValidUntil = DateTime.Now.AddDays(7)
            };

            var command = new AddTaskCommand(addTaskDto, "user@test.com");

            // Resolve the mediator from DI
            var mediator = _serviceProvider.GetRequiredService<IMediator>();

            // Act
            var result = await mediator.Send(command);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(addTaskDto.Name, result.Name);
            Assert.Equal(addTaskDto.Description, result.Description);
            Assert.Equal(addTaskDto.IsDone, result.IsDone);
            Assert.Equal(addTaskDto.ValidUntil, result.ValidUntil);

            // Assert the task is persisted in the database
            var taskInDb = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Name == addTaskDto.Name);
            Assert.NotNull(taskInDb);
            Assert.Equal(addTaskDto.Description, taskInDb.Description);
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            _userQueriesRepoMock.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(value: null);

            var addTaskDto = new AddTaskDto
            {
                Name = "Test Task",
                Description = "Test Task Description",
                IsDone = false,
                ValidUntil = DateTime.Now.AddDays(7)
            };

            var command = new AddTaskCommand(addTaskDto, "nonexistentuser@test.com");

            // Resolve the mediator from DI
            var mediator = _serviceProvider.GetRequiredService<IMediator>();

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                mediator.Send(command));
        }

        // Clean up resources after each test
        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();  // Delete the database after each test
            _dbContext.Dispose();                 // Dispose of the DbContext
            ((IDisposable)_serviceProvider).Dispose(); // Dispose of the service provider
        }
    }

}
