using FluentValidation;

namespace FSTD.Application.DTOs.Productivity.Tasks
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
        public DateTime ValidUntil { get; set; }
    }
    public class TaskDtoValidator : AbstractValidator<TaskDto>
    {
        public TaskDtoValidator()
        {
            RuleFor(dto => dto.Id)
                .NotEmpty().WithMessage("Id is required.");

            // Validate that Name is not empty and has a maximum length of 100 characters
            RuleFor(dto => dto.Name)
                .NotEmpty()
                .WithMessage("Task name is required.")
                .MaximumLength(100)
                .WithMessage("Task name cannot exceed 100 characters.");

            // Validate that Description is optional but if provided, has a maximum length of 500 characters
            RuleFor(dto => dto.Description)
                .MaximumLength(500)
                .WithMessage("Task description cannot exceed 500 characters.");

            // Validate the ValidUntil date to be a future date
            RuleFor(dto => dto.ValidUntil)
                .GreaterThanOrEqualTo(DateTime.Now.Date)
                .WithMessage("ValidUntil must be a future date.");

        }
    }
}
