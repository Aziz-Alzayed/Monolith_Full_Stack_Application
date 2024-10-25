using FSTD.Application.DTOs.Accounts.Users;

namespace FSTD.Application.Unit.Tests.DTOs.Accounts.Users
{
    public class UpdateUserEmailDtoValidatorTests
    {
        private readonly UpdateUserEmailDtoValidator _validator;

        public UpdateUserEmailDtoValidatorTests()
        {
            _validator = new UpdateUserEmailDtoValidator(); // Initialize the validator
        }

        // Test cases for NewEmail
        [Theory]
        [InlineData(null, "Email is required.")]
        [InlineData("", "Email is required.")]
        [InlineData("invalid-email", "Email must be a valid email address.")]
        public void Should_Have_Error_When_NewEmail_Is_Invalid(string newEmail, string expectedErrorMessage)
        {
            // Arrange
            var dto = new UpdateUserEmailDto
            {
                NewEmail = newEmail,
                VerificationUrl = "https://validurl.com/verify" // Valid URL for this test case
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NewEmail)
                .WithErrorMessage(expectedErrorMessage);
        }

        // Test cases for VerificationUrl
        [Theory]
        [InlineData(null, "Return URL is required.")]
        [InlineData("", "Return URL is required.")]
        [InlineData("invalid-url", "URL must start with https:// or http://")]
        public void Should_Have_Error_When_VerificationUrl_Is_Invalid(string verificationUrl, string expectedErrorMessage)
        {
            // Arrange
            var dto = new UpdateUserEmailDto
            {
                NewEmail = "valid@example.com", // Valid email for this test case
                VerificationUrl = verificationUrl
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.VerificationUrl)
                .WithErrorMessage(expectedErrorMessage);
        }

        // Test case for valid inputs
        [Fact]
        public void Should_Not_Have_Error_When_NewEmail_And_VerificationUrl_Are_Valid()
        {
            // Arrange
            var dto = new UpdateUserEmailDto
            {
                NewEmail = "valid@example.com",
                VerificationUrl = "https://validurl.com/verify"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.NewEmail);
            result.ShouldNotHaveValidationErrorFor(x => x.VerificationUrl);
        }
    }

}
