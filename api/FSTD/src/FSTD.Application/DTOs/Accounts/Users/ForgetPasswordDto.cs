using FluentValidation;
using FSTD.Application.Validations.Extentions;

namespace FSTD.Application.DTOs.Accounts.Users
{
    public class ForgetPasswordDto
    {
        public string Email { get; set; }
        public string ResetUrl { get; set; }
    }
    public class ForgetPasswordDtoValidator : AbstractValidator<ForgetPasswordDto>
    {
        public ForgetPasswordDtoValidator()
        {
            RuleFor(dto => dto.Email)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .IsEmail();
            RuleFor(dto => dto.ResetUrl)
                .NotEmpty().WithMessage("Return URL is required.")
                .IsReturnURL();
        }
    }
}
