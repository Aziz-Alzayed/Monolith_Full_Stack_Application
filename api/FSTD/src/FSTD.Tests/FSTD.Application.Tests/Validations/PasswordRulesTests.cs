using FSTD.Application.Validations;

namespace FSTD.Application.Unit.Tests.Validations
{
    public class PasswordRulesTests
    {
        [Fact]
        public void Should_ContainRequiredLengthInRequirements()
        {
            // Arrange & Act
            var requirements = PasswordRules.GetPasswordRequirements();

            // Assert
            Assert.Contains($"Must be at least {PasswordRules.RequiredLength} characters long", requirements);
        }

        [Fact]
        public void Should_ContainDigitRequirement_When_RequireDigitIsTrue()
        {
            // Arrange & Act
            var requirements = PasswordRules.GetPasswordRequirements();

            // Assert
            if (PasswordRules.RequireDigit)
            {
                Assert.Contains("At least one number", requirements);
            }
            else
            {
                Assert.DoesNotContain("At least one number", requirements);
            }
        }

        [Fact]
        public void Should_ContainLowercaseRequirement_When_RequireLowercaseIsTrue()
        {
            // Arrange & Act
            var requirements = PasswordRules.GetPasswordRequirements();

            // Assert
            if (PasswordRules.RequireLowercase)
            {
                Assert.Contains("At least one lower case character", requirements);
            }
            else
            {
                Assert.DoesNotContain("At least one lower case character", requirements);
            }
        }

        [Fact]
        public void Should_ContainUppercaseRequirement_When_RequireUppercaseIsTrue()
        {
            // Arrange & Act
            var requirements = PasswordRules.GetPasswordRequirements();

            // Assert
            if (PasswordRules.RequireUppercase)
            {
                Assert.Contains("At least one upper case character", requirements);
            }
            else
            {
                Assert.DoesNotContain("At least one upper case character", requirements);
            }
        }

        [Fact]
        public void Should_ContainSpecialCharacterRequirement_When_RequireNonAlphanumericIsTrue()
        {
            // Arrange & Act
            var requirements = PasswordRules.GetPasswordRequirements();

            // Assert
            if (PasswordRules.RequireNonAlphanumeric)
            {
                Assert.Contains("At least one special character", requirements);
            }
            else
            {
                Assert.DoesNotContain("At least one special character", requirements);
            }
        }

        [Fact]
        public void Should_ContainCorrectNumberOfRequirements()
        {
            // Arrange & Act
            var requirements = PasswordRules.GetPasswordRequirements();

            // Assert
            var expectedCount = 1; // "Must be at least X characters long" is always present
            if (PasswordRules.RequireDigit) expectedCount++;
            if (PasswordRules.RequireLowercase) expectedCount++;
            if (PasswordRules.RequireUppercase) expectedCount++;
            if (PasswordRules.RequireNonAlphanumeric) expectedCount++;

            Assert.Equal(expectedCount, requirements.Count);
        }
    }
}
