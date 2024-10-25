using FSTD.Application.DTOs.Accounts.Users;

namespace FSTD.Application.Unit.Tests.DTOs.Accounts.Users
{
    public class ForgetPasswordDtoValidatorTests
    {
        private readonly ForgetPasswordDtoValidator _validator;

        public ForgetPasswordDtoValidatorTests()
        {
            _validator = new ForgetPasswordDtoValidator(); // Initialize the validator
        }

        // Test cases for Email
        [Theory]
        [InlineData(null, "Email is required.")]
        [InlineData("", "Email is required.")]
        [InlineData("invalid-email", "Email must be a valid email address.")] // Assuming IsEmail validator handles invalid formats
        public void Should_Have_Error_When_Email_Is_Invalid(string email, string expectedErrorMessage)
        {
            // Arrange
            var dto = new ForgetPasswordDto
            {
                Email = email,
                ResetUrl = "https://validurl.com/reset" // Valid URL for this test case
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage(expectedErrorMessage);
        }

        // Test cases for ResetUrl
        [Theory]
        [InlineData(null, "Return URL is required.")]
        [InlineData("", "Return URL is required.")]
        [InlineData("invalid-url", "URL must start with https:// or http://")] // Assuming IsReturnURL validator handles invalid URL formats
        public void Should_Have_Error_When_ResetUrl_Is_Invalid(string resetUrl, string expectedErrorMessage)
        {
            // Arrange
            var dto = new ForgetPasswordDto
            {
                Email = "valid@example.com", // Valid email for this test case
                ResetUrl = resetUrl
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ResetUrl)
                .WithErrorMessage(expectedErrorMessage);
        }

        // Test case for valid inputs
        [Fact]
        public void Should_Not_Have_Error_When_Email_And_ResetUrl_Are_Valid()
        {
            // Arrange
            var dto = new ForgetPasswordDto
            {
                Email = "valid@example.com",
                ResetUrl = "https://validurl.com/reset"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
            result.ShouldNotHaveValidationErrorFor(x => x.ResetUrl);
        }
    }
}
