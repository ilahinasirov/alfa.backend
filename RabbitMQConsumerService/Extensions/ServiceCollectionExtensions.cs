using Candidate.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using RabbitMQConsumerService.Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQConsumerService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>();
        }

        public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Common
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IActionInvoker>();
            var connectionString = configuration.GetConnectionString("PostgresConnection");
            services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
        );
            #endregion

            RegisterConsumers(services);
        }

        private static void RegisterConsumers(this IServiceCollection services)
        {
            services.AddSingleton<CreateVacancyConsumer>();
            services.AddHostedService<CreateVacancyConsumer>();
        }
    }
}
