using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace BetFriend.AzureFunctions
{
    public static class ConfigurationExtensions
    {
        public static IFunctionsHostBuilder UseAppSettings(this IFunctionsHostBuilder hostBuilder)
        {
            hostBuilder.UseAppSettings(x => x.AddEnvironmentVariables());
            return hostBuilder;
        }

        public static IFunctionsHostBuilder AddDependencies(this IFunctionsHostBuilder builder)
        {
            builder.Services
                   //.AddLogging()
                   .AddApplicationInsightsTelemetry(builder.GetContext().Configuration["ApplicationInsightKey"]);
            return builder;
        }

        private static IFunctionsHostBuilder UseAppSettings(this IFunctionsHostBuilder hostBuilder, Action<IConfigurationBuilder> configurationAction)
        {
            var configurationBuilder = new ConfigurationBuilder();
            var configurationRoot = hostBuilder.Services.BuildServiceProvider().GetService<IConfiguration>();
            if (configurationRoot != null)
            {
                configurationBuilder.AddConfiguration(configurationRoot);
            }

            configurationAction?.Invoke(configurationBuilder);
            var configuration = configurationBuilder.Build();
            hostBuilder.Services.Replace(ServiceDescriptor.Singleton(typeof(IConfiguration), configuration));
            return hostBuilder;
        }
    }
}
