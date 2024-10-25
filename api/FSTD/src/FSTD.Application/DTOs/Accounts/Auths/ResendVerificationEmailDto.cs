using FluentValidation;
using FSTD.Application.Validations.Extentions;

namespace FSTD.Application.DTOs.Accounts.Auths
{
    public class ResendVerificationEmailDto
    {
        public string UserEmail { get; set; }
        public string VerificationUrl { get; set; }
    }
    public class ResendVerificationEmailDtoValidator : AbstractValidator<ResendVerificationEmailDto>
    {
        public ResendVerificationEmailDtoValidator()
        {
            RuleFor(dto => dto.UserEmail)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .IsEmail();
            RuleFor(dto => dto.VerificationUrl)
                .NotEmpty().WithMessage("Return URL is required.")
                .IsReturnURL();
        }
    }
}
