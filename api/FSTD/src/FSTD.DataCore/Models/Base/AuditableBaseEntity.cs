using FSTD.DataCore.Models.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSTD.DataCore.Models.Base
{
    public abstract class AuditableBaseEntity : Base
    {
        public Guid? CreatedByUserId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public Guid? ModifiedByUserId { get; set; }
        public DateTime ModifiedOn { get; set; }
        public virtual ApplicationUser CreatedByUser { get; set; }
        public virtual ApplicationUser ModifiedByUser { get; set; }
    }
    public static class BaseClassRelationships
    {
        public static void ApplyRelationships<T>(EntityTypeBuilder<T> entity) where T : AuditableBaseEntity
        {
            BaseRelationships.ApplyRelationships(entity);

            entity.HasOne(d => d.CreatedByUser)
                .WithMany()
                .HasForeignKey(d => d.CreatedByUserId)
                .HasPrincipalKey(u => u.Id);

            entity.HasOne(d => d.ModifiedByUser)
                .WithMany()
                .HasForeignKey(d => d.ModifiedByUserId)
                .HasPrincipalKey(u => u.Id);
        }

        public static T Prefill<T>(ApplicationUser user, T entity, DateTime? date = null) where T : AuditableBaseEntity
        {
            var dateInp = date ?? DateTime.UtcNow;
            entity.ModifiedOn = dateInp;
            entity.CreatedOn = dateInp;
            entity.CreatedByUserId = user.Id;
            entity.ModifiedByUserId = user.Id;
            return entity;
        }

        public static T Updated<T>(ApplicationUser user, T entity, DateTime? date = null) where T : AuditableBaseEntity
        {
            var dateInp = date ?? DateTime.UtcNow;
            entity.ModifiedOn = dateInp;
            entity.ModifiedByUserId = user.Id;
            return entity;
        }
    }
}
