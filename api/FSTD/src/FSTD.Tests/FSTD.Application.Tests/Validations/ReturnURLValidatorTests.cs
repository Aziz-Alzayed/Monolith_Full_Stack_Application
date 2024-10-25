using FSTD.Application.Validations;

namespace FSTD.Application.Unit.Tests.Validations
{
    public class ReturnURLValidatorTests
    {
        private readonly ReturnURLValidator _validator;

        public ReturnURLValidatorTests()
        {
            _validator = new ReturnURLValidator();
        }

        [Fact]
        public void Should_NotHaveError_When_ReturnURLIsValidHttps()
        {
            // Arrange
            var returnUrl = "https://example.com";

            // Act
            var result = _validator.TestValidate(returnUrl);

            // Assert
            result.ShouldNotHaveValidationErrorFor(url => url);
        }

        [Fact]
        public void Should_NotHaveError_When_ReturnURLIsValidHttp()
        {
            // Arrange
            var returnUrl = "http://example.com"; // Testing if the validation allows both http and https

            // Act
            var result = _validator.TestValidate(returnUrl);

            // Assert
            result.ShouldNotHaveValidationErrorFor(url => url);
        }

        [Fact]
        public void Should_HaveError_When_ReturnURLDoesNotStartWithHttpOrHttps()
        {
            // Arrange
            var returnUrl = "ftp://example.com"; // Invalid protocol

            // Act
            var result = _validator.TestValidate(returnUrl);

            // Assert
            result.ShouldHaveValidationErrorFor(url => url)
                .WithErrorMessage("URL must start with https:// or http://");
        }
    }
}
