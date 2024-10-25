using AutoMapper;
using FSTD.Application.DTOs.Accounts.Admins;
using FSTD.Application.MediatoR.Accounts.Admins.Queries;
using FSTD.Application.MediatoR.Accounts.Admins.Repos;
using FSTD.Application.Models.Admin;
using FSTD.DataCore.Models.Users;
using FSTD.Infrastructure.MediatoR.Accounts.Admins.Queries;
using FSTD.Infrastructure.MediatoR.Accounts.Services;
using Moq;

namespace FSTD.Infrastructure.Unit.Tests.MediatoR.Accounts.Admins.Queries
{
    public class GetAllUsersQueryHandlerTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<IAdminQueriesRepo> _adminQueriesRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllUsersQueryHandler _handler;

        public GetAllUsersQueryHandlerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _adminQueriesRepoMock = new Mock<IAdminQueriesRepo>();
            _mapperMock = new Mock<IMapper>();

            _handler = new GetAllUsersQueryHandler(
                _accountServiceMock.Object,
                _adminQueriesRepoMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_List_Of_Users_When_User_Has_Permission()
        {
            // Arrange
            var requestedBy = "admin@test.com";
            var query = new GetAllUsersQuery(requestedBy);

            var usersWithRoles = new List<UserWithRolesModel>
        {
            new UserWithRolesModel
            {
                User = new ApplicationUser { FirstName = "John", LastName = "Doe", Email = "john.doe@test.com" },
                Roles = new List<string> { "Admin" }
            }
        };

            var mappedUsers = new List<UserFullInfoDto>
        {
            new UserFullInfoDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@test.com",
                Roles = new List<string> { "Admin" }
            }
        };

            // Mock permission check to return true
            _accountServiceMock.Setup(service => service.CheckPermissionsIsAdminOrSuperAsync(requestedBy))
                               .ReturnsAsync(true);

            // Mock getting users with roles
            _adminQueriesRepoMock.Setup(repo => repo.GetAllApplicationUsersWithRolesAsync())
                                 .ReturnsAsync(usersWithRoles);

            // Mock mapping from UserWithRolesModel to UserFullInfoDto
            _mapperMock.Setup(mapper => mapper.Map<List<UserFullInfoDto>>(usersWithRoles))
                       .Returns(mappedUsers);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("John", result[0].FirstName);
            Assert.Equal("Doe", result[0].LastName);
            Assert.Equal("john.doe@test.com", result[0].Email);

            // Verify the mock interactions
            _accountServiceMock.Verify(service => service.CheckPermissionsIsAdminOrSuperAsync(requestedBy), Times.Once);
            _adminQueriesRepoMock.Verify(repo => repo.GetAllApplicationUsersWithRolesAsync(), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<List<UserFullInfoDto>>(usersWithRoles), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_UnauthorizedAccessException_When_User_Does_Not_Have_Permission()
        {
            // Arrange
            var requestedBy = "user@test.com";
            var query = new GetAllUsersQuery(requestedBy);

            // Mock permission check to return false
            _accountServiceMock.Setup(service => service.CheckPermissionsIsAdminOrSuperAsync(requestedBy))
                               .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(query, CancellationToken.None));

            // Verify the mock interactions
            _accountServiceMock.Verify(service => service.CheckPermissionsIsAdminOrSuperAsync(requestedBy), Times.Once);
            _adminQueriesRepoMock.Verify(repo => repo.GetAllApplicationUsersWithRolesAsync(), Times.Never);
            _mapperMock.Verify(mapper => mapper.Map<List<UserFullInfoDto>>(It.IsAny<List<UserWithRolesModel>>()), Times.Never);
        }
    }

}
