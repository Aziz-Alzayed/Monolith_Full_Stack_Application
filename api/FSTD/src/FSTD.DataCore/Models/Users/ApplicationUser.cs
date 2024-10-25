using FSTD.DataCore.Models.ProductivityModels;
using Microsoft.AspNetCore.Identity;


namespace FSTD.DataCore.Models.Users
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public ICollection<TasksModel> Tasks { get; set; }
    }
}
