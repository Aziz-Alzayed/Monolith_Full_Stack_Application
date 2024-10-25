using FSTD.Application.MediatoR.Accounts.Admins.Commands;
using FSTD.Application.MediatoR.Accounts.Admins.Repos;
using FSTD.DataCore.Models.Users;
using FSTD.Infrastructure.MediatoR.Accounts.Admins.Commands;
using FSTD.Infrastructure.MediatoR.Accounts.Services;
using Moq;

namespace FSTD.Infrastructure.Unit.Tests.MediatoR.Accounts.Admins.Commands
{

    public class DeleteUserByIdCommandHandlerTests
    {
        private readonly Mock<IAdminCommandsRepo> _adminCommandsRepoMock;
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly DeleteUserByIdCommandHandler _handler;

        public DeleteUserByIdCommandHandlerTests()
        {
            _adminCommandsRepoMock = new Mock<IAdminCommandsRepo>();
            _accountServiceMock = new Mock<IAccountService>();

            _handler = new DeleteUserByIdCommandHandler(
                _adminCommandsRepoMock.Object,
                _accountServiceMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Delete_User_When_Deleted_By_SuperUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var editedByEmail = "superuser@test.com";

            var user = new ApplicationUser { Id = userId, Email = "user@test.com" };
            var editedBy = new ApplicationUser { Email = editedByEmail };

            var command = new DeleteUserByIdCommand(userId, editedByEmail);

            // Mocking the account service methods
            _accountServiceMock.Setup(service => service.GetUserAsync(userId)).ReturnsAsync(user);
            _accountServiceMock.Setup(service => service.GetUserByEmailAsync(editedByEmail)).ReturnsAsync(editedBy);
            _accountServiceMock.Setup(service => service.IsSuperUserAsync(editedBy)).ReturnsAsync(true); // Super user
            _accountServiceMock.Setup(service => service.IsAdminUserAsync(editedBy)).ReturnsAsync(false);
            _accountServiceMock.Setup(service => service.IsSuperUserAsync(user)).ReturnsAsync(false);
            _accountServiceMock.Setup(service => service.IsAdminUserAsync(user)).ReturnsAsync(false);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _adminCommandsRepoMock.Verify(repo => repo.DeleteUserByIdAsync(user), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Delete_User_When_Deleted_By_Admin_And_User_Is_Not_Admin_Or_Super()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var editedByEmail = "admin@test.com";

            var user = new ApplicationUser { Id = userId, Email = "user@test.com" };
            var editedBy = new ApplicationUser { Email = editedByEmail };

            var command = new DeleteUserByIdCommand(userId, editedByEmail);

            // Mocking the account service methods
            _accountServiceMock.Setup(service => service.GetUserAsync(userId)).ReturnsAsync(user);
            _accountServiceMock.Setup(service => service.GetUserByEmailAsync(editedByEmail)).ReturnsAsync(editedBy);
            _accountServiceMock.Setup(service => service.IsSuperUserAsync(editedBy)).ReturnsAsync(false);
            _accountServiceMock.Setup(service => service.IsAdminUserAsync(editedBy)).ReturnsAsync(true); // Admin user
            _accountServiceMock.Setup(service => service.IsSuperUserAsync(user)).ReturnsAsync(false);
            _accountServiceMock.Setup(service => service.IsAdminUserAsync(user)).ReturnsAsync(false);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _adminCommandsRepoMock.Verify(repo => repo.DeleteUserByIdAsync(user), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_UnauthorizedAccessException_When_Deleted_By_Admin_And_User_Is_Admin_Or_Super()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var editedByEmail = "admin@test.com";

            var user = new ApplicationUser { Id = userId, Email = "adminuser@test.com" };
            var editedBy = new ApplicationUser { Email = editedByEmail };

            var command = new DeleteUserByIdCommand(userId, editedByEmail);

            // Mocking the account service methods
            _accountServiceMock.Setup(service => service.GetUserAsync(userId)).ReturnsAsync(user);
            _accountServiceMock.Setup(service => service.GetUserByEmailAsync(editedByEmail)).ReturnsAsync(editedBy);
            _accountServiceMock.Setup(service => service.IsSuperUserAsync(editedBy)).ReturnsAsync(false);
            _accountServiceMock.Setup(service => service.IsAdminUserAsync(editedBy)).ReturnsAsync(true); // Admin user
            _accountServiceMock.Setup(service => service.IsSuperUserAsync(user)).ReturnsAsync(false);
            _accountServiceMock.Setup(service => service.IsAdminUserAsync(user)).ReturnsAsync(true); // Target user is also admin

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
            _adminCommandsRepoMock.Verify(repo => repo.DeleteUserByIdAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Throw_UnauthorizedAccessException_When_Not_Super_Or_Admin()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var editedByEmail = "user@test.com";

            var user = new ApplicationUser { Id = userId, Email = "adminuser@test.com" };
            var editedBy = new ApplicationUser { Email = editedByEmail };

            var command = new DeleteUserByIdCommand(userId, editedByEmail);

            // Mocking the account service methods
            _accountServiceMock.Setup(service => service.GetUserAsync(userId)).ReturnsAsync(user);
            _accountServiceMock.Setup(service => service.GetUserByEmailAsync(editedByEmail)).ReturnsAsync(editedBy);
            _accountServiceMock.Setup(service => service.IsSuperUserAsync(editedBy)).ReturnsAsync(false);
            _accountServiceMock.Setup(service => service.IsAdminUserAsync(editedBy)).ReturnsAsync(false); // Not admin or super
            _accountServiceMock.Setup(service => service.IsSuperUserAsync(user)).ReturnsAsync(false);
            _accountServiceMock.Setup(service => service.IsAdminUserAsync(user)).ReturnsAsync(true); // Target user is admin

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
            _adminCommandsRepoMock.Verify(repo => repo.DeleteUserByIdAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }
    }
}
