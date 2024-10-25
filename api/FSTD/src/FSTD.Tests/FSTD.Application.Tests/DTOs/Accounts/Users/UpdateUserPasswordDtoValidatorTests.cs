using FSTD.Application.DTOs.Accounts.Users;

namespace FSTD.Application.Unit.Tests.DTOs.Accounts.Users
{
    public class UpdateUserPasswordDtoValidatorTests
    {
        private readonly UpdateUserPasswordDtoValidator _validator;

        public UpdateUserPasswordDtoValidatorTests()
        {
            _validator = new UpdateUserPasswordDtoValidator(); // Initialize the validator
        }

        // Test cases for OldPassword
        [Theory]
        [InlineData(null, "OldPassword is required.")]
        [InlineData("", "OldPassword is required.")]
        public void Should_Have_Error_When_OldPassword_Is_Invalid(string oldPassword, string expectedErrorMessage)
        {
            // Arrange
            var dto = new UpdateUserPasswordDto
            {
                OldPassword = oldPassword,
                NewPassword = "ValidPassword123!" // Valid NewPassword for this test case
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.OldPassword)
                .WithErrorMessage(expectedErrorMessage);
        }

        // Test cases for NewPassword
        [Theory]
        [InlineData(null, "NewPassword cannot be null.")]
        [InlineData("", "NewPassword name is required.")]
        [InlineData("short", "Password must be at least 10 characters long.")]
        public void Should_Have_Error_When_NewPassword_Is_Invalid(string newPassword, string expectedErrorMessage)
        {
            // Arrange
            var dto = new UpdateUserPasswordDto
            {
                OldPassword = "ValidOldPassword123!", // Valid OldPassword for this test case
                NewPassword = newPassword
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NewPassword)
                .WithErrorMessage(expectedErrorMessage);
        }

        // Test case for valid inputs
        [Fact]
        public void Should_Not_Have_Error_When_OldPassword_And_NewPassword_Are_Valid()
        {
            // Arrange
            var dto = new UpdateUserPasswordDto
            {
                OldPassword = "ValidOldPassword123!",
                NewPassword = "ValidNewPassword123!"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.OldPassword);
            result.ShouldNotHaveValidationErrorFor(x => x.NewPassword);
        }
    }

}
