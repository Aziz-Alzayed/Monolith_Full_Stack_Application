using FluentValidation;

namespace FSTD.Application.DTOs.Accounts.Auths
{
    public class RefreshTokenRequestDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
    public class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshTokenRequestDto>
    {
        public RefreshTokenRequestDtoValidator()
        {
            RuleFor(dto => dto.AccessToken)
                .NotEmpty().WithMessage("Access Token must not be empty.");
            RuleFor(dto => dto.RefreshToken)
                .NotEmpty().WithMessage("Refresh Token must not be empty.");
        }
    }
}
