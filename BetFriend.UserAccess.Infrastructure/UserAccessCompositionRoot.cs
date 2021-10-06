namespace BetFriend.UserAccess.Infrastructure
{
    using Microsoft.Extensions.DependencyInjection;
    using System;


    internal static class UserAccessCompositionRoot
    {
        private static IServiceProvider _serviceProvider;

        internal static void SetProvider(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        internal static IServiceScope BeginScope()
        {
            return _serviceProvider.CreateScope();
        }
    }
}
