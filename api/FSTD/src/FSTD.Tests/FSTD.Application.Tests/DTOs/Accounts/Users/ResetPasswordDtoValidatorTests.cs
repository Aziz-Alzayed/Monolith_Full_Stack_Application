using FSTD.Application.DTOs.Accounts.Users;

namespace FSTD.Application.Unit.Tests.DTOs.Accounts.Users
{
    public class ResetPasswordDtoValidatorTests
    {
        private readonly ResetPasswordDtoValidator _validator;

        public ResetPasswordDtoValidatorTests()
        {
            _validator = new ResetPasswordDtoValidator(); // Initialize the validator
        }

        // Test cases for Email
        [Theory]
        [InlineData(null, "Email is required.")]
        [InlineData("", "Email is required.")]
        [InlineData("invalid-email", "Email must be a valid email address.")]  // Assuming IsEmail validator handles invalid formats
        public void Should_Have_Error_When_Email_Is_Invalid(string email, string expectedErrorMessage)
        {
            // Arrange
            var dto = new ResetPasswordDto
            {
                Email = email,
                Token = "ValidToken",
                NewPassword = "ValidPassword123!"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage(expectedErrorMessage);
        }

        // Test cases for Token
        [Theory]
        [InlineData(null, "Token name is required.")]
        [InlineData("", "Token name is required.")]
        public void Should_Have_Error_When_Token_Is_Invalid(string token, string expectedErrorMessage)
        {
            // Arrange
            var dto = new ResetPasswordDto
            {
                Email = "valid@example.com",
                Token = token,
                NewPassword = "ValidPassword123!"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Token)
                .WithErrorMessage(expectedErrorMessage);
        }

        // Test cases for NewPassword
        [Theory]
        [InlineData(null, "Password is required.")]
        [InlineData("", "Password is required.")]
        [InlineData("short", "Password must be at least 10 characters long.")]
        public void Should_Have_Error_When_NewPassword_Is_Invalid(string newPassword, string expectedErrorMessage)
        {
            // Arrange
            var dto = new ResetPasswordDto
            {
                Email = "valid@example.com",
                Token = "ValidToken",
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
        public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
        {
            // Arrange
            var dto = new ResetPasswordDto
            {
                Email = "valid@example.com",
                Token = "ValidToken",
                NewPassword = "ValidPassword123!"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
            result.ShouldNotHaveValidationErrorFor(x => x.Token);
            result.ShouldNotHaveValidationErrorFor(x => x.NewPassword);
        }
    }

}
