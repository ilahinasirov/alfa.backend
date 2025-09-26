
using Microsoft.EntityFrameworkCore;
using Domain.Identity;
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Domain.Constant;

namespace Infrastructure.DataAccess
{
    public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IAppDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<SystemConstant> SystemConstants { get; set; }

        public async Task AsTransactionAsync(Func<Task> func, CancellationToken cancellationToken = default)
        {
            if (Database.CurrentTransaction != null) await func();

            await using var transaction = await Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await func();
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<T> AsTransactionAsync<T>(Func<Task<T>> func, CancellationToken cancellationToken = default)
        {
            if (Database.CurrentTransaction != null) return await func();

            await using var transaction = await Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var result = await func();
                await transaction.CommitAsync(cancellationToken);
                return result;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
        warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        //protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        //{
        //    configurationBuilder.ConfigureSmartEnum();
        //    base.ConfigureConventions(configurationBuilder);
        //}
    }
}
