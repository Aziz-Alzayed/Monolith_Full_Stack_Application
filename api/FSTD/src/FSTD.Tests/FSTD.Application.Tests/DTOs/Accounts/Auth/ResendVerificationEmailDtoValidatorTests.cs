﻿using FSTD.Application.DTOs.Accounts.Auths;

namespace FSTD.Application.Unit.Tests.DTOs.Accounts.Auth
{
    public class ResendVerificationEmailDtoValidatorTests
    {
        private readonly ResendVerificationEmailDtoValidator _validator;

        public ResendVerificationEmailDtoValidatorTests()
        {
            _validator = new ResendVerificationEmailDtoValidator(); // Initialize the validator
        }

        // Test cases for UserEmail
        [Theory]
        [InlineData(null, "Email is required.")]
        [InlineData("", "Email is required.")]
        [InlineData("invalid-email", "Email must be a valid email address.")] // Assuming IsEmail validator handles invalid formats
        public void Should_Have_Error_When_UserEmail_Is_Invalid(string userEmail, string expectedErrorMessage)
        {
            // Arrange
            var dto = new ResendVerificationEmailDto
            {
                UserEmail = userEmail,
                VerificationUrl = "https://validurl.com" // Valid URL for this test case
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserEmail)
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
            var dto = new ResendVerificationEmailDto
            {
                UserEmail = "valid@example.com", // Valid email for this test case
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
        public void Should_Not_Have_Error_When_Email_And_VerificationUrl_Are_Valid()
        {
            // Arrange
            var dto = new ResendVerificationEmailDto
            {
                UserEmail = "valid@example.com",
                VerificationUrl = "https://validurl.com/verify"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UserEmail);
            result.ShouldNotHaveValidationErrorFor(x => x.VerificationUrl);
        }
    }

}
