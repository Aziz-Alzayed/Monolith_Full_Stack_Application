using FluentValidation;
using FSTD.Application.Validations.Extentions;

namespace FSTD.Application.DTOs.Accounts.Users
{
    public class UpdateUserPasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class UpdateUserPasswordDtoValidator : AbstractValidator<UpdateUserPasswordDto>
    {
        public UpdateUserPasswordDtoValidator()
        {
            RuleFor(dto => dto.OldPassword)
                .NotEmpty().WithMessage("OldPassword is required.");
            RuleFor(dto => dto.NewPassword)
                .NotNull().WithMessage("NewPassword cannot be null.")
                .NotEmpty().WithMessage("NewPassword name is required.")
                .IsPassword();
        }
    }
}
