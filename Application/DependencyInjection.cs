using System.Reflection;
using Application.Common.Behaviours;
using Application.Common.Implementation;
using Application.Common.Interfaces;
using FluentValidation;
using MessagePack.Resolvers;
using MessagePack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Application.Configuration;
using Application.Services;
using Application.UseCases.Constant;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationCore(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            });

            services.AddServices();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IWordDocumentExtensionsService, WordDocumentExtensionsService>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IMessageQueueService, RabbitMQProducer>();

            services.AddScoped<Lazy<IIdentityService>>((provider) =>
                new Lazy<IIdentityService>(provider.GetRequiredService<IIdentityService>));

            return services;
        }


        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            MessagePackSerializer.DefaultOptions = MessagePackSerializerOptions.Standard
                .WithResolver(ContractlessStandardResolver.Instance);

            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!));

            services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));


            services.AddHostedService<CreateUserConsumer>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateSystemConstantCommand).Assembly));

            services.AddScoped(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase());

            //services.AddScoped<IDistributedCacheService, RedisCacheService>();

            return services;
        }

        public static IServiceCollection AddMinioClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IStorageService, MinioStorageService>();

            return services;
        }

    }
}
