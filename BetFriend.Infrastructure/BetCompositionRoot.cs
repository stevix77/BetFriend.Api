namespace BetFriend.Bet.Infrastructure
{
    using Microsoft.Extensions.DependencyInjection;
    using System;

    internal static class BetCompositionRoot
    {
        private static IServiceProvider _serviceProvider;

        internal static IServiceScope BeginScope()
        {
            return _serviceProvider.CreateScope();
        }

        internal static void SetProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
    }
}
