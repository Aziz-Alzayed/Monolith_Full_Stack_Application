using FSTD.Application.DTOs.Accounts.Auths;

namespace FSTD.Application.Unit.Tests.DTOs.Accounts.Auth
{
    public class LoginRequestDtoValidatorTests
    {
        private readonly LoginRequestDtoValidator _validator;

        public LoginRequestDtoValidatorTests()
        {
            _validator = new LoginRequestDtoValidator(); // Initialize the validator
        }

        // Test cases for Password
        [Theory]
        [InlineData(null, "Password is required.")]
        [InlineData("", "Password is required.")]
        [InlineData("short", "Password must be at least 10 characters long.")]
        public void Should_Have_Error_When_Password_Is_Invalid(string password, string expectedErrorMessage)
        {
            // Arrange
            var dto = new LoginRequestDto
            {
                Email = "test@example.com",
                Password = password
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage(expectedErrorMessage);
        }

        // Test cases for Email
        [Theory]
        [InlineData(null, "Email is required.")]
        [InlineData("", "Email is required.")]
        [InlineData("invalid-email", "Email must be a valid email address.")]
        public void Should_Have_Error_When_Email_Is_Invalid(string email, string expectedErrorMessage)
        {
            // Arrange
            var dto = new LoginRequestDto
            {
                Email = email,
                Password = "ValidPassword123!" // Valid password for this test case
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage(expectedErrorMessage);
        }

        // Test case for valid inputs
        [Fact]
        public void Should_Not_Have_Error_When_Email_And_Password_Are_Valid()
        {
            // Arrange
            var dto = new LoginRequestDto
            {
                Email = "test@example.com",
                Password = "ValidPassword123!"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }
    }
}
