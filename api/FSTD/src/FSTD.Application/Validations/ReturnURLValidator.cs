using FluentValidation;

namespace FSTD.Application.Validations
{
    public class ReturnURLValidator : AbstractValidator<string>
    {

        public ReturnURLValidator()
        {
            RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .Must(BeAValidHttpsUrl).WithMessage(@"URL must start with https:// or http://");
        }

        private bool BeAValidHttpsUrl(string returnUrl)
        {
            return !string.IsNullOrEmpty(returnUrl) &&
                   (returnUrl.StartsWith("https://") || returnUrl.StartsWith("http://"));
        }
    }
}
