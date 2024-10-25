namespace FSTD.Application.Validations
{
    public static class PasswordRules
    {
        public const int RequiredLength = 10;
        public const bool RequireDigit = true;
        public const bool RequireLowercase = true;
        public const bool RequireNonAlphanumeric = true;
        public const bool RequireUppercase = true;
        public const int RequiredUniqueChars = 1;

        public static List<string> GetPasswordRequirements()
        {
            var reqs = new List<string>
        {
            $"Must be at least {RequiredLength} characters long"
        };
            if (RequireDigit)
            {
                reqs.Add("At least one number");
            }
            if (RequireLowercase)
            {
                reqs.Add("At least one lower case character");
            }
            if (RequireUppercase)
            {
                reqs.Add("At least one upper case character");
            }
            if (RequireNonAlphanumeric)
            {
                reqs.Add("At least one special character");
            }
            return reqs;
        }
    }
}
