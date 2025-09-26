using Application.Common.Implementation;
using Application.Common.Interfaces;
using Application.Configuration;
using Infrastructure.Configuration;
using Infrastructure.DataAccess.Configurations;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQSettings = Application.Configuration.RabbitMQSettings;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services,
        ConfigurationManager configuration)
        {
            services.AddDataAccess(configuration);

            // Bind configuration and register config models
            var rabbitMQSettings = new RabbitMQSettings();
            configuration.GetSection("RabbitMQ").Bind(rabbitMQSettings);
            services.AddSingleton(rabbitMQSettings);

            services.Configure<RedisOptions>(configuration.GetSection("RedisOptions"));
            services.Configure<OtpOptions>(configuration.GetSection("OtpOptions"));
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

            // Register RabbitMQProducer
            services.AddSingleton<IMessageQueueService, RabbitMQProducer>();
            services.AddSingleton<IRedisService, RedisService>();
            services.AddSingleton<IMailService, MailService>();

            return services;
        }
    }
}
