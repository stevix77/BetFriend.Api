using Microsoft.Extensions.DependencyInjection;
using System;

namespace BetFriend.Bet.Infrastructure
{
    internal static class BetCompositionRoot
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
