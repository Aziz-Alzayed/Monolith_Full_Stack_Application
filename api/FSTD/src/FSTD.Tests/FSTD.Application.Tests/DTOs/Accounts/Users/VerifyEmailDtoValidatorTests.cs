using FSTD.Application.DTOs.Accounts.Users;

namespace FSTD.Application.Unit.Tests.DTOs.Accounts.Users
{
    public class VerifyEmailDtoValidatorTests
    {
        private readonly VerifyEmailDtoValidator _validator;

        public VerifyEmailDtoValidatorTests()
        {
            _validator = new VerifyEmailDtoValidator(); // Initialize the validator
        }

        // Test cases for UserId
        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", "UserId is required.")] // Empty GUID
        public void Should_Have_Error_When_UserId_Is_Invalid(Guid userId, string expectedErrorMessage)
        {
            // Arrange
            var dto = new VerifyEmailDto
            {
                UserId = userId,
                Token = "ValidToken123!" // Valid Token for this test case
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId)
                .WithErrorMessage(expectedErrorMessage);
        }

        // Test cases for Token
        [Theory]
        [InlineData(null, "Token is required.")]
        [InlineData("", "Token is required.")]
        public void Should_Have_Error_When_Token_Is_Invalid(string token, string expectedErrorMessage)
        {
            // Arrange
            var dto = new VerifyEmailDto
            {
                UserId = Guid.NewGuid(), // Valid UserId for this test case
                Token = token
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Token)
                .WithErrorMessage(expectedErrorMessage);
        }

        // Test case for valid inputs
        [Fact]
        public void Should_Not_Have_Error_When_UserId_And_Token_Are_Valid()
        {
            // Arrange
            var dto = new VerifyEmailDto
            {
                UserId = Guid.NewGuid(), // Valid UserId
                Token = "ValidToken123!" // Valid Token
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
            result.ShouldNotHaveValidationErrorFor(x => x.Token);
        }
    }

}
