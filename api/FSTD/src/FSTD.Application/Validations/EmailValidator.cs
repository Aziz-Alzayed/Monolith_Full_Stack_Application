using FluentValidation;

namespace FSTD.Application.Validations
{
    public class EmailValidator : AbstractValidator<string>
    {
        public EmailValidator()
        {
            RuleFor(email => email)
                .Cascade(CascadeMode.Stop)
                .EmailAddress().WithMessage("Email must be a valid email address.") // FluentValidation's built-in email check
                .Must(HaveValidEmailStructure).WithMessage("Email must be a valid email address."); // Custom rule to ensure "@" and "." are present
        }

        // Custom validation to check if the email contains "@" and a "."
        private bool HaveValidEmailStructure(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            var atIndex = email.IndexOf('@');
            var dotIndex = email.LastIndexOf('.');

            // Ensure "@" is before the last "." and both exist
            return atIndex > 0 && dotIndex > atIndex + 1 && dotIndex < email.Length - 1;
        }
    }
}
