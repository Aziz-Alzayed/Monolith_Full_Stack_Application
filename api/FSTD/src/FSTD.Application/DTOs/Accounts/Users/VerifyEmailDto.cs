using FluentValidation;

namespace FSTD.Application.DTOs.Accounts.Users
{
    public class VerifyEmailDto
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
    public class VerifyEmailDtoValidator : AbstractValidator<VerifyEmailDto>
    {
        public VerifyEmailDtoValidator()
        {
            RuleFor(dto => dto.UserId)
                .NotEmpty().WithMessage("UserId is required.");
            RuleFor(dto => dto.Token)
                .NotEmpty().WithMessage("Token is required.");
        }
    }
}
