using FluentValidation;
using FSTD.Application.Validations.Extentions;

namespace FSTD.Application.DTOs.Accounts.Users
{
    public class RequestResetPasswordDto
    {
        public string Email { get; set; }
        public string ResetURL { get; set; }
    }
    public class RequestResetPasswordDtoValidator : AbstractValidator<RequestResetPasswordDto>
    {
        public RequestResetPasswordDtoValidator()
        {
            RuleFor(dto => dto.Email)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .IsEmail();
            RuleFor(dto => dto.ResetURL)
                .NotEmpty().WithMessage("Return URL is required.")
                .IsReturnURL();

        }
    }
}
