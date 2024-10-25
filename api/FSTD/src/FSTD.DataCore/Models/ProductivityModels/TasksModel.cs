using FSTD.DataCore.Models.Users;

namespace FSTD.DataCore.Models.ProductivityModels
{
    public class TasksModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
        public DateTime ValidUntil { get; set; }
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
