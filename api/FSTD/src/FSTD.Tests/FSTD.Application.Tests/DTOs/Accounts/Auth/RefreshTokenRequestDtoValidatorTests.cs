using FSTD.Application.DTOs.Accounts.Auths;

namespace FSTD.Application.Unit.Tests.DTOs.Accounts.Auth
{
    public class RefreshTokenRequestDtoValidatorTests
    {
        private readonly RefreshTokenRequestDtoValidator _validator;

        public RefreshTokenRequestDtoValidatorTests()
        {
            _validator = new RefreshTokenRequestDtoValidator();
        }

        // Test cases for AccessToken
        [Theory]
        [InlineData(null, "Access Token must not be empty.")]
        [InlineData("", "Access Token must not be empty.")]
        public void Should_Have_Error_When_AccessToken_Is_Invalid(string accessToken, string expectedErrorMessage)
        {
            // Arrange
            var dto = new RefreshTokenRequestDto
            {
                AccessToken = accessToken,
                RefreshToken = "ValidRefreshToken" // Valid refresh token for this test case
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.AccessToken)
                .WithErrorMessage(expectedErrorMessage);
        }

        // Test cases for RefreshToken
        [Theory]
        [InlineData(null, "Refresh Token must not be empty.")]
        [InlineData("", "Refresh Token must not be empty.")]
        public void Should_Have_Error_When_RefreshToken_Is_Invalid(string refreshToken, string expectedErrorMessage)
        {
            // Arrange
            var dto = new RefreshTokenRequestDto
            {
                AccessToken = "ValidAccessToken", // Valid access token for this test case
                RefreshToken = refreshToken
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RefreshToken)
                .WithErrorMessage(expectedErrorMessage);
        }

        // Test case for valid inputs
        [Fact]
        public void Should_Not_Have_Error_When_AccessToken_And_RefreshToken_Are_Valid()
        {
            // Arrange
            var dto = new RefreshTokenRequestDto
            {
                AccessToken = "ValidAccessToken",
                RefreshToken = "ValidRefreshToken"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.AccessToken);
            result.ShouldNotHaveValidationErrorFor(x => x.RefreshToken);
        }
    }

}
