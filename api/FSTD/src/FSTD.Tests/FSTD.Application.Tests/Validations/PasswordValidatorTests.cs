using FSTD.Application.Validations;

namespace FSTD.Application.Unit.Tests.Validations
{
    public class PasswordValidatorTests
    {
        private readonly PasswordValidator _validator;

        public PasswordValidatorTests()
        {
            _validator = new PasswordValidator();
        }

        [Fact]
        public void Should_HaveError_When_PasswordIsTooShort()
        {
            // Arrange
            var password = new string('a', PasswordRules.RequiredLength - 1);

            // Act
            var result = _validator.TestValidate(password);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p)
                .WithErrorMessage($"Password must be at least {PasswordRules.RequiredLength} characters long.");
        }

        [Fact]
        public void Should_HaveError_When_PasswordDoesNotContainUppercaseLetter()
        {
            // Arrange
            var password = "password123!"; // no uppercase letter

            // Act
            var result = _validator.TestValidate(password);

            // Assert
            if (PasswordRules.RequireUppercase)
            {
                result.ShouldHaveValidationErrorFor(p => p)
                    .WithErrorMessage("Password must contain an uppercase letter.");
            }
            else
            {
                result.ShouldNotHaveValidationErrorFor(p => p);
            }
        }

        [Fact]
        public void Should_HaveError_When_PasswordDoesNotContainLowercaseLetter()
        {
            // Arrange
            var password = "PASSWORD123!"; // no lowercase letter

            // Act
            var result = _validator.TestValidate(password);

            // Assert
            if (PasswordRules.RequireLowercase)
            {
                result.ShouldHaveValidationErrorFor(p => p)
                    .WithErrorMessage("Password must contain a lowercase letter.");
            }
            else
            {
                result.ShouldNotHaveValidationErrorFor(p => p);
            }
        }

        [Fact]
        public void Should_HaveError_When_PasswordDoesNotContainNonAlphanumericCharacter()
        {
            // Arrange
            var password = "Password123"; // no non-alphanumeric character

            // Act
            var result = _validator.TestValidate(password);

            // Assert
            if (PasswordRules.RequireNonAlphanumeric)
            {
                result.ShouldHaveValidationErrorFor(p => p)
                    .WithErrorMessage("Password must contain a non-alphanumeric character.");
            }
            else
            {
                result.ShouldNotHaveValidationErrorFor(p => p);
            }
        }

        [Fact]
        public void Should_NotHaveError_When_PasswordIsValid()
        {
            // Arrange
            var password = "Password123!"; // valid password

            // Act
            var result = _validator.TestValidate(password);

            // Assert
            result.ShouldNotHaveValidationErrorFor(p => p);
        }
    }
}
