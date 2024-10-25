using FSTD.Application.DTOs.Accounts.Admins;

namespace FSTD.Application.Unit.Tests.DTOs.Accounts.Admins
{
    public class UpdateUserDtoValidatorTests
    {
        private readonly UpdateUserDtoValidator _validator;

        public UpdateUserDtoValidatorTests()
        {
            _validator = new UpdateUserDtoValidator();
        }

        // Test case for when the Id is null or empty
        [Fact]
        public void Should_Have_Error_When_Id_Is_Empty()
        {
            // Arrange
            var dto = new UpdateUserDto { Id = Guid.Empty };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Id is required.");
        }

        // Test case for FirstName validation
        [Fact]
        public void Should_Have_Error_When_FirstName_Is_Empty()
        {
            // Arrange
            var dto = new UpdateUserDto { FirstName = "" };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                .WithErrorMessage("First name is required.");
        }

        [Theory]
        [InlineData("A")] // Too short
        [InlineData("VeryLongFirstNameThatExceedsTheCharacterLimitOfFiftyCharacters")] // Too long
        public void Should_Have_Error_When_FirstName_Is_Out_Of_Length_Range(string firstName)
        {
            // Arrange
            var dto = new UpdateUserDto { FirstName = firstName };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                .WithErrorMessage("First name must be between 2 and 50 characters.");
        }

        // Test case for LastName validation
        [Fact]
        public void Should_Have_Error_When_LastName_Is_Null()
        {
            // Arrange
            var dto = new UpdateUserDto { LastName = null };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                .WithErrorMessage("Last Name cannot be null.");
        }

        [Fact]
        public void Should_Have_Error_When_LastName_Is_Empty()
        {
            // Arrange
            var dto = new UpdateUserDto { LastName = "" };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                .WithErrorMessage("Last name is required.");
        }

        [Theory]
        [InlineData("A")] // Too short
        [InlineData("VeryLongLastNameThatExceedsTheCharacterLimitOfFiftyCharacters")] // Too long
        public void Should_Have_Error_When_LastName_Is_Out_Of_Length_Range(string lastName)
        {
            // Arrange
            var dto = new UpdateUserDto { LastName = lastName };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                .WithErrorMessage("Last name must be between 2 and 50 characters.");
        }

        // Test case for PhoneNumber validation
        [Fact]
        public void Should_Have_Error_When_PhoneNumber_Is_Null()
        {
            // Arrange
            var dto = new UpdateUserDto
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "test@example.com",
                PhoneNumber = null,
                Roles = new string[] { "Admin" }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                .WithErrorMessage("Phone Number cannot be null.");
        }

        [Fact]
        public void Should_Have_Error_When_PhoneNumber_Is_Invalid()
        {
            // Arrange
            var dto = new UpdateUserDto { PhoneNumber = "+0123456789" }; // Invalid phone number

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                .WithErrorMessage("Invalid phone number format.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_PhoneNumber_Is_Valid()
        {
            // Arrange
            var dto = new UpdateUserDto { PhoneNumber = "+1234567890" }; // Valid phone number

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
        }

        // Test case for Email validation
        [Theory]
        [InlineData("invalid-email")]
        [InlineData("test@.com")]
        [InlineData("test@domain")]
        public void Should_Have_Error_When_Email_Is_Invalid(string email)
        {
            // Arrange
            var dto = new UpdateUserDto { Email = email };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email must be a valid email address.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Email_Is_Valid()
        {
            // Arrange
            var dto = new UpdateUserDto { Email = "user@example.com" }; // Valid email

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        // Test case for Roles validation
        [Fact]
        public void Should_Have_Error_When_Roles_Is_Null()
        {
            // Arrange
            var dto = new UpdateUserDto { Roles = null };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Roles)
                .WithErrorMessage("Roles cannot be null.");
        }

        [Fact]
        public void Should_Have_Error_When_Roles_Are_Empty()
        {
            // Arrange
            var dto = new UpdateUserDto { Roles = new string[] { } };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Roles)
                .WithErrorMessage("At least one role is required.");
        }

        [Fact]
        public void Should_Have_Error_When_Roles_Contain_Empty_Values()
        {
            // Arrange
            var dto = new UpdateUserDto { Roles = new[] { "Admin", "" } };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Roles)
                .WithErrorMessage("Roles cannot contain an empty value.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Roles_Are_Valid()
        {
            // Arrange
            var dto = new UpdateUserDto { Roles = new[] { "Admin", "User" } };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Roles);
        }
    }
}
