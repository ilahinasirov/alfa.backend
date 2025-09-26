using Shared.Models;
using Shared.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPortalSettings(this IServiceCollection services, IConfiguration configuration)
        {
            AddCommonSettings(services, configuration);
            return services;
        }

        private static void AddCommonSettings(this IServiceCollection services, IConfiguration configuration)
        {
            Configure<AuthSettings>(services, configuration, AuthSettings.SectionName);
            Configure<FileServerSettings>(services, configuration, FileServerSettings.SectionName);

        }

        private static void Configure<T>(IServiceCollection services, IConfiguration configuration,
            string? sectionName = null) where T : class, new()
        {
            services.Configure<T>(configuration.GetSection(sectionName ?? typeof(T).Name));
            services.AddSingleton(provider => provider.GetRequiredService<IOptions<T>>().Value);
        }

        public static void UseLocalization(this IApplicationBuilder app)
        {
            var supportedCultures = new[] { "az", "en" };

            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);
        }
    }
}
