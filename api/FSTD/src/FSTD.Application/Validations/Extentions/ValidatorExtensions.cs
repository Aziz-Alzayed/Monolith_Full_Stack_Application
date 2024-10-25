using FluentValidation;

namespace FSTD.Application.Validations.Extentions
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilder<T, string> IsPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new PasswordValidator());
        }

        public static IRuleBuilder<T, string> IsEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new EmailValidator());
        }
        public static IRuleBuilder<T, string> IsReturnURL<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new ReturnURLValidator());
        }
        public static IRuleBuilder<T, string> IsPhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new PhoneNumberValidator());
        }
    }
}
