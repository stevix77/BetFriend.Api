namespace BetFriend.Bet.Infrastructure
{
    using Microsoft.Extensions.DependencyInjection;
    using System;

    internal class BetCompositionRoot
    {
        private readonly IServiceProvider _serviceProvider;
        internal BetCompositionRoot(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        internal IServiceScope BeginScope()
        {
            return _serviceProvider.CreateScope();
        }
    }
}
