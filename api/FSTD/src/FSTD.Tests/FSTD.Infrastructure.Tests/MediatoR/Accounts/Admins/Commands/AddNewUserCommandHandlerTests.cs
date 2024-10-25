using AutoMapper;
using FSTD.Application.DTOs.Accounts.Admins;
using FSTD.Application.MediatoR.Accounts.Admins.Commands;
using FSTD.Application.MediatoR.Accounts.Admins.Repos;
using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.Application.Models.Admin;
using FSTD.DataCore.Models.Users;
using FSTD.Infrastructure.MediatoR.Accounts.Admins.Commands;
using FSTD.Infrastructure.MediatoR.Accounts.Auth.Services;
using FSTD.Infrastructure.MediatoR.Accounts.Services;
using Moq;

namespace FSTD.Infrastructure.Unit.Tests.MediatoR.Accounts.Admins.Commands
{
    public class AddNewUserCommandHandlerTests
    {
        private readonly Mock<IAdminCommandsRepo> _adminCommandsRepoMock;
        private readonly Mock<IAdminQueriesRepo> _adminQueriesRepoMock;
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<IRolesService> _rolesServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserCommandsRepo> _userCommandsRepoMock;
        private readonly Mock<IAccountEmailSender> _accountEmailSenderMock;
        private readonly AddNewUserCommandHandler _handler;

        public AddNewUserCommandHandlerTests()
        {
            _adminCommandsRepoMock = new Mock<IAdminCommandsRepo>();
            _adminQueriesRepoMock = new Mock<IAdminQueriesRepo>();
            _accountServiceMock = new Mock<IAccountService>();
            _rolesServiceMock = new Mock<IRolesService>();
            _mapperMock = new Mock<IMapper>();
            _userCommandsRepoMock = new Mock<IUserCommandsRepo>();
            _accountEmailSenderMock = new Mock<IAccountEmailSender>();

            _handler = new AddNewUserCommandHandler(
                _adminCommandsRepoMock.Object,
                _adminQueriesRepoMock.Object,
                _accountServiceMock.Object,
                _rolesServiceMock.Object,
                _mapperMock.Object,
                _userCommandsRepoMock.Object,
                _accountEmailSenderMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Add_New_User_When_Valid_Roles_And_CanAssign_True()
        {
            // Arrange
            var addNewUserDto = new AddNewUserDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                Roles = new[] { "Admin" },
                ResetURL = "http://example.com/reset-password"
            };

            var newUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                FirstName = addNewUserDto.FirstName,
                LastName = addNewUserDto.LastName,
                UserName = addNewUserDto.Email,
                Email = addNewUserDto.Email
            };

            var userWithRoles = new UserWithRolesModel
            {
                User = newUser,
                Roles = new List<string> { "Admin" }
            };

            var command = new AddNewUserCommand(addNewUserDto, "admin@test.com");

            // Mock the validation of roles
            _rolesServiceMock.Setup(service => service.ValidateRoles(It.IsAny<List<string>>()))
                             .Returns(new List<string> { "Admin" });

            // Mock the permission to assign roles
            _accountServiceMock.Setup(service => service.CanAssignRolesAsync(It.IsAny<string>(), It.IsAny<string[]>()))
                               .ReturnsAsync(true);

            // Mock the process of adding a new user
            _adminCommandsRepoMock.Setup(repo => repo.AddNewUserAsync(It.IsAny<ApplicationUser>())).Returns(Task.CompletedTask);
            _adminCommandsRepoMock.Setup(repo => repo.AddRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<List<string>>())).Returns(Task.CompletedTask);
            _userCommandsRepoMock.Setup(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<ApplicationUser>())).ReturnsAsync("fake-token");
            _accountEmailSenderMock.Setup(sender => sender.SendPasswordResetEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ApplicationUser>()))
                                   .Returns(Task.CompletedTask);

            // Mock the retrieval of the user with roles
            _adminQueriesRepoMock.Setup(repo => repo.GetlApplicationUserWithRolesAsync(It.IsAny<ApplicationUser>()))
                                 .ReturnsAsync(userWithRoles);

            // Mock the mapping of the result
            _mapperMock.Setup(m => m.Map<UserFullInfoDto>(userWithRoles)).Returns(new UserFullInfoDto());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _adminCommandsRepoMock.Verify(repo => repo.AddNewUserAsync(It.IsAny<ApplicationUser>()), Times.Once);
            _adminCommandsRepoMock.Verify(repo => repo.AddRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<List<string>>()), Times.Once);
            _userCommandsRepoMock.Verify(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<ApplicationUser>()), Times.Once);
            _accountEmailSenderMock.Verify(sender => sender.SendPasswordResetEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ApplicationUser>()), Times.Once);
            _mapperMock.Verify(m => m.Map<UserFullInfoDto>(userWithRoles), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_UnauthorizedAccessException_When_Roles_Are_Invalid()
        {
            // Arrange
            var addNewUserDto = new AddNewUserDto
            {
                Roles = new[] { "InvalidRole" }
            };

            var command = new AddNewUserCommand(addNewUserDto, "admin@test.com");

            // Mock invalid roles
            _rolesServiceMock.Setup(service => service.ValidateRoles(It.IsAny<List<string>>()))
                             .Returns(new List<string>());

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
            _adminCommandsRepoMock.Verify(repo => repo.AddNewUserAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Throw_UnauthorizedAccessException_When_Cannot_Assign_Roles()
        {
            // Arrange
            var addNewUserDto = new AddNewUserDto
            {
                Roles = new[] { "Admin" }
            };

            var command = new AddNewUserCommand(addNewUserDto, "admin@test.com");

            // Mock valid roles
            _rolesServiceMock.Setup(service => service.ValidateRoles(It.IsAny<List<string>>()))
                             .Returns(new List<string> { "Admin" });

            // Mock the failure of assigning roles
            _accountServiceMock.Setup(service => service.CanAssignRolesAsync(It.IsAny<string>(), It.IsAny<string[]>()))
                               .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
            _adminCommandsRepoMock.Verify(repo => repo.AddNewUserAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }
    }
}
