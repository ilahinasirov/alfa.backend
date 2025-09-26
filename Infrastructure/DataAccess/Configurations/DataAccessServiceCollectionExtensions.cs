using Application.Common.Interfaces;
using Infrastructure.DataAccess;
using Infrastructure.DataAccess.Configurations.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure.DataAccess.Configurations
{
    public static class DataAccessServiceCollectionExtensions
    {
        internal static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISaveChangesInterceptor, TimestampInterceptor>();

            var connectionStringBuilder =
                new NpgsqlConnectionStringBuilder(configuration.GetConnectionString("PostgresConnection"))
                {
                    Pooling = true,
                    ApplicationName = configuration["ApplicationName"]
                };
            if (configuration.GetValue<int>("DatabasePoolSize") > 0)
                connectionStringBuilder.MaxPoolSize = configuration.GetValue<int>("DatabasePoolSize");

            services.AddDbContext<IAppDbContext, AppDbContext>((sp, options) =>
            {
                options.UseNpgsql(connectionStringBuilder.ConnectionString);
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            });

            services.BuildServiceProvider().MigrateDatabase<AppDbContext>();

            return services;
        }
    }
}

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=alfa;Username=postgres;Password=Qwerty1");

        return new AppDbContext(optionsBuilder.Options);
    }
}
