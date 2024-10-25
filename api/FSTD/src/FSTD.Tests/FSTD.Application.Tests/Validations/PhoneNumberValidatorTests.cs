using FSTD.Application.Validations;

namespace FSTD.Application.Unit.Tests.Validations
{
    public class PhoneNumberValidatorTests
    {
        private readonly PhoneNumberValidator _validator;

        public PhoneNumberValidatorTests()
        {
            _validator = new PhoneNumberValidator();
        }

        // Test case for when the phone number is null
        [Fact]
        public void Should_Have_Error_When_PhoneNumber_Is_Null()
        {
            // Arrange
            string phoneNumber = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _validator.TestValidate(phoneNumber));
            Assert.Equal("Cannot pass null model to Validate. (Parameter 'instanceToValidate')", exception.Message);
        }


        // Test cases for invalid phone number formats
        [Theory]
        [InlineData("12345")]         // Missing "+"
        [InlineData("+0123456789")]    // Leading zero in country code
        [InlineData("+123")]          // Too short
        [InlineData("+12345678901234567")] // Too long
        public void Should_Have_Error_When_PhoneNumber_Is_Invalid(string phoneNumber)
        {
            // Act
            var result = _validator.TestValidate(phoneNumber);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x)
                  .WithErrorMessage("Invalid phone number format.");
        }

        // Test case for valid phone numbers
        [Theory]
        [InlineData("+1234567890")]     // Valid E.164 format
        [InlineData("+19876543210")]    // Another valid phone number
        public void Should_Not_Have_Error_When_PhoneNumber_Is_Valid(string phoneNumber)
        {
            // Act
            var result = _validator.TestValidate(phoneNumber);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x);
        }
    }
}
