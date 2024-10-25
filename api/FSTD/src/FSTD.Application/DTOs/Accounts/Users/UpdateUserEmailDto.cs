using FluentValidation;
using FSTD.Application.Validations.Extentions;

namespace FSTD.Application.DTOs.Accounts.Users
{
    public class UpdateUserEmailDto
    {
        public string NewEmail { get; set; }
        public string VerificationUrl { get; set; }
    }
    public class UpdateUserEmailDtoValidator : AbstractValidator<UpdateUserEmailDto>
    {
        public UpdateUserEmailDtoValidator()
        {
            RuleFor(dto => dto.NewEmail)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .IsEmail();

            RuleFor(dto => dto.VerificationUrl)
                .NotEmpty().WithMessage("Return URL is required.")
                .IsReturnURL();
        }
    }
}
