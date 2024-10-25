using FSTD.Application.Validations;

namespace FSTD.Application.Unit.Tests.Validations
{

    public class EmailValidatorTests
    {
        private readonly EmailValidator _validator;

        public EmailValidatorTests()
        {
            _validator = new EmailValidator();
        }

        // Test case for when the email is null
        [Fact]
        public void Should_Have_Error_When_Email_Is_Null()
        {
            // Arrange
            string email = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _validator.TestValidate(email));

            // Assert that the exception is thrown
            Assert.Equal("Cannot pass null model to Validate. (Parameter 'instanceToValidate')", exception.Message);
        }

        // Test cases for invalid email formats
        [Theory]
        [InlineData("invalid-email")]
        [InlineData("user@domain")]
        [InlineData("user@.com")]
        [InlineData("@example.com")]
        [InlineData("user@domain.")]
        public void Should_Have_Error_When_Email_Is_Invalid(string email)
        {
            // Act
            var result = _validator.TestValidate(email);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x)
                  .WithErrorMessage("Email must be a valid email address.");
        }

        // Test case for valid email addresses
        [Theory]
        [InlineData("user@example.com")]
        [InlineData("admin@domain.org")]
        [InlineData("contact@company.co")]
        public void Should_Not_Have_Error_When_Email_Is_Valid(string email)
        {
            // Act
            var result = _validator.TestValidate(email);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x);
        }
    }
}
