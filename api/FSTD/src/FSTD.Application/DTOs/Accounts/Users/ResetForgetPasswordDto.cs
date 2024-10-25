using FluentValidation;
using FSTD.Application.Validations.Extentions;

namespace FSTD.Application.DTOs.Accounts.Users
{
    public class ResetForgetPasswordDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
    public class ResetForgetPasswordDtoValidator : AbstractValidator<ResetForgetPasswordDto>
    {
        public ResetForgetPasswordDtoValidator()
        {
            RuleFor(dto => dto.Email)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .IsEmail();
            RuleFor(dto => dto.Token).NotEmpty().WithMessage("Token name is required.");
            RuleFor(dto => dto.NewPassword)
                .NotNull().WithMessage("Password is required.")
                .NotEmpty().WithMessage("Password is required.")
                .IsPassword();
        }
    }
}
