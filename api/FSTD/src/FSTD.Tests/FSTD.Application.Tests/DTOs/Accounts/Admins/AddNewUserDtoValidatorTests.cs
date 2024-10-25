using FSTD.Application.DTOs.Accounts.Admins;

namespace FSTD.Application.Unit.Tests.DTOs.Accounts.Admins
{
    public class AddNewUserDtoValidatorTests
    {
        private readonly AddNewUserDtoValidator _validator;

        public AddNewUserDtoValidatorTests()
        {
            _validator = new AddNewUserDtoValidator();
        }

        [Fact]
        public void Should_Have_Error_When_FirstName_Is_Null()
        {
            // Arrange
            var dto = new AddNewUserDto
            {
                FirstName = null,
                Roles = new string[] { "Admin" }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                .WithErrorMessage("First name cannot be null.");
        }

        [Fact]
        public void Should_Have_Error_When_FirstName_Is_Empty()
        {
            // Arrange
            var dto = new AddNewUserDto
            {
                FirstName = "",
                Roles = new string[] { "Admin" }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                .WithErrorMessage("First name is required.");
        }

        [Theory]
        [InlineData("A")]
        [InlineData("VeryLongFirstNameThatExceedsTheCharacterLimitOfFiftyCharacters")]
        public void Should_Have_Error_When_FirstName_Is_Out_Of_Length_Range(string firstName)
        {
            // Arrange
            var dto = new AddNewUserDto
            {
                FirstName = firstName,
                Roles = new string[] { "Admin" }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                .WithErrorMessage("First name must be between 2 and 50 characters.");
        }

        [Fact]
        public void Should_Pass_When_FirstName_Is_Valid()
        {
            // Arrange
            var dto = new AddNewUserDto
            {
                FirstName = "John",
                Roles = new string[] { "Admin" }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Should_Have_Error_When_LastName_Is_Null()
        {
            // Arrange
            var dto = new AddNewUserDto
            {
                LastName = null,
                Roles = new string[] { "Admin" }
            };

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
            var dto = new AddNewUserDto
            {
                LastName = "",
                Roles = new string[] { "Admin" }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                .WithErrorMessage("Last name is required.");
        }

        [Theory]
        [InlineData("A")]
        [InlineData("VeryLongLastNameThatExceedsTheCharacterLimitOfFiftyCharacters")]
        public void Should_Have_Error_When_LastName_Is_Out_Of_Length_Range(string lastName)
        {
            // Arrange
            var dto = new AddNewUserDto
            {
                LastName = lastName,
                Roles = new string[] { "Admin" }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                .WithErrorMessage("Last name must be between 2 and 50 characters.");
        }

        [Fact]
        public void Should_Pass_When_LastName_Is_Valid()
        {
            // Arrange
            var dto = new AddNewUserDto
            {
                LastName = "Doe",
                Roles = new string[] { "Admin" }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void Should_Have_Error_When_PhoneNumber_Is_Null()
        {
            // Arrange
            var dto = new AddNewUserDto
            {
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
        public void Should_Have_Error_When_PhoneNumber_Is_Empty()
        {
            // Arrange
            var dto = new AddNewUserDto
            {
                PhoneNumber = "",
                Roles = new string[] { "Admin" }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                .WithErrorMessage("Phone number is required.");
        }

        [Theory]
        [InlineData("12345")]
        [InlineData("+123")]
        [InlineData("++123456789")]
        public void Should_Have_Error_When_PhoneNumber_Format_Is_Invalid(string phoneNumber)
        {
            // Arrange
            var dto = new AddNewUserDto
            {
                PhoneNumber = phoneNumber,
                Roles = new string[] { "Admin" }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                .WithErrorMessage("Invalid phone number format.");
        }

        [Fact]
        public void Should_Pass_When_PhoneNumber_Is_Valid()
        {
            // Arrange
            var dto = new AddNewUserDto
            {
                PhoneNumber = "+12345678901",
                Roles = new string[] { "Admin" }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Null()
        {
            // Arrange
            var dto = new AddNewUserDto
            {
                Email = "",
                Roles = new string[] { "Admin" }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email is required.");
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Empty()
        {
            // Arrange
            var dto = new AddNewUserDto
            {
                Email = "",
                Roles = new string[] { "Admin" }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email is required.");
        }

        [Theory]
        [InlineData("invalid-email", new[] { "Admin" })]
        [InlineData("test@.com", new[] { "Admin" })]
        [InlineData("test@domain", new[] { "Admin" })]
        public void Should_Have_Error_When_Email_Is_Invalid(string email, string[] roles)
        {
            // Arrange
            var dto = new AddNewUserDto { Email = email, Roles = roles };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email must be a valid email address.");  // Adjust message to match your validator
        }

        [Fact]
        public void Should_Pass_When_Email_Is_Valid()
        {
            // Arrange
            var dto = new AddNewUserDto
            {
                Email = "test@test.com",
                Roles = new string[] { "Admin" }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Have_Error_When_Roles_Is_Null()
        {
            // Arrange
            var dto = new AddNewUserDto { Roles = null };

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
            var dto = new AddNewUserDto { Roles = new string[] { } };

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
            var dto = new AddNewUserDto { Roles = new[] { "Admin", "" } };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Roles)
                .WithErrorMessage("Roles cannot contain an empty value.");
        }

        [Fact]
        public void Should_Pass_When_Roles_Are_Valid()
        {
            // Arrange
            var dto = new AddNewUserDto { Roles = new[] { "Admin", "User" } };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Roles);
        }

        [Fact]
        public void Should_Have_Error_When_ResetURL_Is_Invalid()
        {
            // Arrange
            var dto = new AddNewUserDto
            {
                ResetURL = "invalid-url",
                Roles = new string[] { "Admin" }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ResetURL)
                .WithErrorMessage("URL must start with https:// or http://");
        }

        [Fact]
        public void Should_Pass_When_ResetURL_Is_Valid()
        {
            // Arrange
            var dto = new AddNewUserDto
            {
                ResetURL = "https://valid-return-url.com",
                Roles = new string[] { "Admin" }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.ResetURL);
        }
    }
}
