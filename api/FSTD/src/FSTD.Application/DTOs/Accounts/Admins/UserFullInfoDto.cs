namespace FSTD.Application.DTOs.Accounts.Admins
{
    public class UserFullInfoDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public List<string> Roles { get; set; }
    }
}
