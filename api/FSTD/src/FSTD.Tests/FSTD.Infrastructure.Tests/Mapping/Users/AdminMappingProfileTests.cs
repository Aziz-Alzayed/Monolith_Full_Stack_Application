using AutoMapper;
using FSTD.Application.DTOs.Accounts.Admins;
using FSTD.Application.Models.Admin;
using FSTD.DataCore.Models.Users;
using FSTD.Infrastructure.Mapping.Users;

namespace FSTD.Infrastructure.Unit.Tests.Mapping.Users
{
    public class AdminMappingProfileTests
    {
        private readonly IMapper _mapper;

        public AdminMappingProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AdminMappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void UserWithRolesModel_To_UserFullInfoDto_Mapping_Is_Valid()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                EmailConfirmed = true
            };

            var userWithRoles = new UserWithRolesModel
            {
                User = user,
                Roles = new List<string> { "Admin", "User" }
            };

            // Act
            var userFullInfoDto = _mapper.Map<UserFullInfoDto>(userWithRoles);

            // Assert
            Assert.Equal(user.Id, userFullInfoDto.Id);
            Assert.Equal(user.FirstName, userFullInfoDto.FirstName);
            Assert.Equal(user.LastName, userFullInfoDto.LastName);
            Assert.Equal(user.Email, userFullInfoDto.Email);
            Assert.Equal(user.PhoneNumber, userFullInfoDto.PhoneNumber);
            Assert.Equal(user.EmailConfirmed, userFullInfoDto.IsEmailConfirmed);
            Assert.Equal(userWithRoles.Roles, userFullInfoDto.Roles);
        }
    }
}
