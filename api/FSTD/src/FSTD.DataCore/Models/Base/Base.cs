using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSTD.DataCore.Models.Base
{
    public class Base
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

    }
    public static class BaseRelationships
    {
        public static void ApplyRelationships<T>(EntityTypeBuilder<T> entity) where T : Base
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        }
    }
}
