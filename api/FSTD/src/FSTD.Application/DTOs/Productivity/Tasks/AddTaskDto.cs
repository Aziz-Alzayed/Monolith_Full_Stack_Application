using FluentValidation;

namespace FSTD.Application.DTOs.Productivity.Tasks
{
    public class AddTaskDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
        public DateTime ValidUntil { get; set; }
    }
    public class AddTaskDtoValidator : AbstractValidator<AddTaskDto>
    {
        public AddTaskDtoValidator()
        {
            // Validate that Name is not empty and has a maximum length of 100 characters
            RuleFor(dto => dto.Name)
                .NotEmpty()
                .WithMessage("Task name is required.")
                .MaximumLength(100)
                .WithMessage("Task name cannot exceed 100 characters.");

            // Validate that Description is optional but if provided, it has a maximum length of 500 characters
            RuleFor(dto => dto.Description)
                .MaximumLength(500)
                .WithMessage("Task description cannot exceed 500 characters.");

            // Validate that ValidUntil date is in the future
            RuleFor(dto => dto.ValidUntil.Date)
                .GreaterThanOrEqualTo(DateTime.Now.Date)
                .WithMessage("ValidUntil must be a future date.");
        }
    }
}
