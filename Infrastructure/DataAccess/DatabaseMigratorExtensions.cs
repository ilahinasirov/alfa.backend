using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DataAccess
{
    public static class DatabaseMigratorExtensions
    {
        public static void MigrateDatabase<T>(this IServiceProvider serviceProvider) where T : DbContext
        {
            using var scope = serviceProvider.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<T>();
            if (dbContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
                return;

            dbContext.Database.Migrate();
        }
    }
}
