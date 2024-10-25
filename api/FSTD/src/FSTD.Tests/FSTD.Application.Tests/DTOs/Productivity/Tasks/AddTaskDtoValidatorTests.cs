using FSTD.Application.DTOs.Productivity.Tasks;

namespace FSTD.Application.Unit.Tests.DTOs.Productivity.Tasks
{
    public class AddTaskDtoValidatorTests
    {
        private readonly AddTaskDtoValidator _validator;

        public AddTaskDtoValidatorTests()
        {
            _validator = new AddTaskDtoValidator(); // Initialize the validator
        }

        // Test cases for Name
        [Theory]
        [InlineData(null, "Task name is required.")]
        [InlineData("", "Task name is required.")]
        [InlineData("A very long task name that exceeds the maximum allowed length of 100 characters, and it keeps going and going.", "Task name cannot exceed 100 characters.")]
        public void Should_Have_Error_When_Name_Is_Invalid(string name, string expectedErrorMessage)
        {
            // Arrange
            var dto = new AddTaskDto
            {
                Name = name,
                Description = "Valid description",
                IsDone = false,
                ValidUntil = DateTime.Now.AddDays(1) // Valid future date
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage(expectedErrorMessage);
        }

        // Test cases for Description
        [Theory]
        [InlineData("A very long description that exceeds the maximum allowed length of 500 characters. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.A very long description that exceeds the maximum allowed length of 500 characters.characters. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.A very long description that exceeds the maximum allowed length of 500 character ")]
        public void Should_Have_Error_When_Description_Exceeds_MaxLength(string description)
        {
            // Arrange
            var dto = new AddTaskDto
            {
                Name = "Valid Task Name",
                Description = description,
                IsDone = false,
                ValidUntil = DateTime.Now.AddDays(1) // Valid future date
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage("Task description cannot exceed 500 characters.");
        }

        // Test cases for ValidUntil
        [Theory]
        [InlineData("2023-01-01")] // A past date
        public void Should_Have_Error_When_ValidUntil_Is_In_The_Past(string validUntil)
        {
            // Arrange
            var dto = new AddTaskDto
            {
                Name = "Valid Task Name",
                Description = "Valid description",
                IsDone = false,
                ValidUntil = DateTime.Parse(validUntil) // Invalid past date
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ValidUntil.Date)
                .WithErrorMessage("ValidUntil must be a future date.");
        }

        // Test case for valid inputs
        [Fact]
        public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
        {
            // Arrange
            var dto = new AddTaskDto
            {
                Name = "Valid Task Name",
                Description = "Valid description",
                IsDone = false,
                ValidUntil = DateTime.Now.AddDays(1) // Valid future date
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
            result.ShouldNotHaveValidationErrorFor(x => x.ValidUntil);
        }
    }

}
