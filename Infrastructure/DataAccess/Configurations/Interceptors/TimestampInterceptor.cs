using Application.Common.Interfaces;
using Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.DataAccess.Configurations.Interceptors
{
    public class TimestampInterceptor(IDateTimeProvider dateTimeProvider) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            ChangeEntityTimestamps(eventData);

            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            ChangeEntityTimestamps(eventData);

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ChangeEntityTimestamps(DbContextEventData eventData)
        {
            var now = dateTimeProvider.UtcNow;

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry.Entity is not IEntity entity)
                    continue;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreatedAt = now;
                        entity.UpdatedAt = now;
                        break;
                    case EntityState.Modified:
                        entity.UpdatedAt = now;
                        break;
                }
            }
        }
    }
}
