using FSTD.DataCore.Models;
using FSTD.DataCore.Models.Base;

namespace FSTD.Infrastructure.MediatoR.Common
{
    internal static class SharedCommandsRepositories
    {
        internal static void EntityCreated<T>(this ApplicationDbContext context, T entity, string addedBy) where T : AuditableBaseEntity
        {
            var user = context.Users.SingleOrDefault(u => u.UserName == addedBy);
            if (user == null)
            {
                throw new UnauthorizedAccessException($"User '{addedBy}' not found.");
            }

            try
            {
                BaseClassRelationships.Prefill(user, entity, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                // Consider logging the exception if you have a logging mechanism
                throw new Exception("An unexpected error occurred while creating the entity.", ex);
            }
        }
        internal static void EntityUpdated<T>(this ApplicationDbContext context, T entity, string updatedBy) where T : AuditableBaseEntity
        {
            var user = context.Users.SingleOrDefault(u => u.UserName == updatedBy);
            if (user == null)
            {
                throw new UnauthorizedAccessException($"User '{updatedBy}' not found.");
            }

            try
            {
                BaseClassRelationships.Updated(user, entity, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                // Consider logging the exception if you have a logging mechanism
                throw new Exception("An unexpected error occurred while updating the entity.", ex);
            }
        }
    }
}
