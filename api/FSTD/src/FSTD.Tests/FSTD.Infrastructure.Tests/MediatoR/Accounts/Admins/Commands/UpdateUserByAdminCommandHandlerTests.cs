using FSTD.Application.DTOs.Accounts.Admins;
using FSTD.Application.MediatoR.Accounts.Admins.Commands;
using FSTD.Application.MediatoR.Accounts.Admins.Repos;
using FSTD.DataCore.Models.Users;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using FSTD.Infrastructure.EmailServices;
using FSTD.Infrastructure.MediatoR.Accounts.Admins.Commands;
using FSTD.Infrastructure.MediatoR.Accounts.Auth.Services;
using FSTD.Infrastructure.MediatoR.Accounts.Services;
using Moq;

namespace FSTD.Infrastructure.Unit.Tests.MediatoR.Accounts.Admins.Commands
{
    public class UpdateUserByAdminCommandHandlerTests
    {
        private readonly Mock<IAdminCommandsRepo> _adminCommandsRepoMock;
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<IRolesService> _rolesServiceMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly UpdateUserByAdminCommandHandler _handler;

        public UpdateUserByAdminCommandHandlerTests()
        {
            _adminCommandsRepoMock = new Mock<IAdminCommandsRepo>();
            _accountServiceMock = new Mock<IAccountService>();
            _rolesServiceMock = new Mock<IRolesService>();
            _emailServiceMock = new Mock<IEmailService>();

            _handler = new UpdateUserByAdminCommandHandler(
                _adminCommandsRepoMock.Object,
                _accountServiceMock.Object,
                _rolesServiceMock.Object,
                _emailServiceMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Update_User_When_Valid_Roles_And_CanAssign_True()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updatedBy = "admin@test.com";

            var updateUserDto = new UpdateUserDto
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                Roles = new[] { "Admin" } // Roles as array
            };

            var user = new ApplicationUser
            {
                Id = userId,
                FirstName = "OldFirstName",
                LastName = "OldLastName",
                Email = "old.email@example.com"
            };

            var command = new UpdateUserByAdminCommand(updateUserDto, updatedBy);

            // Mock user retrieval
            _accountServiceMock.Setup(service => service.GetUserAsync(userId)).ReturnsAsync(user);

            // Mock role validation and convert array to List<string>
            _rolesServiceMock.Setup(service => service.ValidateRoles(It.IsAny<List<string>>()))
                             .Returns(updateUserDto.Roles.ToList()); // Convert array to list

            // Mock permission to assign roles
            _accountServiceMock.Setup(service => service.CanAssignRolesAsync(updatedBy, It.IsAny<string[]>()))
                               .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _adminCommandsRepoMock.Verify(repo => repo.UpdateUserAsync(user), Times.Once);
            _adminCommandsRepoMock.Verify(repo => repo.UpdateRolesAsync(user, It.IsAny<List<string>>()), Times.Once); // Ensure it gets a List<string>
            _emailServiceMock.Verify(service => service.SendEmailAsync(user.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            // Check if user details were updated
            Assert.Equal("John", user.FirstName);
            Assert.Equal("Doe", user.LastName);
            Assert.Equal("john.doe@example.com", user.Email);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_User_Not_Found()
        {
            // Arrange
            var command = new UpdateUserByAdminCommand(new UpdateUserDto { Id = Guid.NewGuid() }, "admin@test.com");

            // Mock user not found
            _accountServiceMock.Setup(service => service.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync((ApplicationUser)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
            _adminCommandsRepoMock.Verify(repo => repo.UpdateUserAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Throw_UnauthorizedAccessException_When_Roles_Are_Invalid()
        {
            // Arrange
            var updateUserDto = new UpdateUserDto
            {
                Id = Guid.NewGuid(),
                Roles = new[] { "InvalidRole" } // Roles as array
            };

            var command = new UpdateUserByAdminCommand(updateUserDto, "admin@test.com");

            var user = new ApplicationUser { Id = updateUserDto.Id };

            // Mock user retrieval to return a valid user
            _accountServiceMock.Setup(service => service.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(user);

            // Mock invalid role validation and convert array to List<string>
            _rolesServiceMock.Setup(service => service.ValidateRoles(It.IsAny<List<string>>()))
                             .Returns(new List<string>()); // Return empty list for invalid roles

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
            _adminCommandsRepoMock.Verify(repo => repo.UpdateUserAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Throw_UnauthorizedAccessException_When_Cannot_Assign_Roles()
        {
            // Arrange
            var updateUserDto = new UpdateUserDto
            {
                Id = Guid.NewGuid(),
                Roles = new[] { "Admin" } // Roles as array
            };

            var command = new UpdateUserByAdminCommand(updateUserDto, "admin@test.com");

            var user = new ApplicationUser { Id = updateUserDto.Id };

            // Mock user retrieval to return a valid user
            _accountServiceMock.Setup(service => service.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(user);

            // Mock role validation and convert array to List<string>
            _rolesServiceMock.Setup(service => service.ValidateRoles(It.IsAny<List<string>>()))
                             .Returns(updateUserDto.Roles.ToList()); // Convert array to list

            // Mock permission to assign roles
            _accountServiceMock.Setup(service => service.CanAssignRolesAsync(It.IsAny<string>(), It.IsAny<string[]>()))
                               .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
            _adminCommandsRepoMock.Verify(repo => repo.UpdateUserAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }
    }


}
