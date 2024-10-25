using AutoMapper;
using FSTD.Application.DTOs.Accounts.Users;
using FSTD.DataCore.Models.Users;
using FSTD.Infrastructure.Mapping.Users;

namespace FSTD.Infrastructure.Unit.Tests.Mapping.Users
{
    public class ApplicationUserProfileTests
    {
        private readonly IMapper _mapper;

        public ApplicationUserProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ApplicationUserProfile>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void ApplicationUser_To_UserInfoDto_Mapping_Is_Valid()
        {
            // Arrange
            var applicationUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                EmailConfirmed = true,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(10)
            };

            // Act
            var userInfoDto = _mapper.Map<UserInfoDto>(applicationUser);

            // Assert
            Assert.Equal(applicationUser.Id, userInfoDto.Id);
            Assert.Equal(applicationUser.FirstName, userInfoDto.FirstName);
            Assert.Equal(applicationUser.LastName, userInfoDto.LastName);
            Assert.Equal(applicationUser.Email, userInfoDto.Email);
            Assert.Equal(applicationUser.EmailConfirmed, userInfoDto.EmailConfirmed);
        }

        [Fact]
        public void UserInfoDto_To_ApplicationUser_Mapping_Is_Valid()
        {
            // Arrange
            var userInfoDto = new UserInfoDto
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                EmailConfirmed = false
            };

            // Act
            var applicationUser = _mapper.Map<ApplicationUser>(userInfoDto);

            // Assert
            Assert.Equal(userInfoDto.Id, applicationUser.Id);
            Assert.Equal(userInfoDto.FirstName, applicationUser.FirstName);
            Assert.Equal(userInfoDto.LastName, applicationUser.LastName);
            Assert.Equal(userInfoDto.Email, applicationUser.Email);
            Assert.Equal(userInfoDto.EmailConfirmed, applicationUser.EmailConfirmed);
        }
    }

}
