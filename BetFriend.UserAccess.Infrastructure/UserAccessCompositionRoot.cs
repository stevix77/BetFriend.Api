namespace BetFriend.UserAccess.Infrastructure
{
    using Microsoft.Extensions.DependencyInjection;
    using System;


    internal class UserAccessCompositionRoot
    {
        internal UserAccessCompositionRoot(IServiceProvider provider)
        {
            _serviceProvider = provider;
        }

        private readonly IServiceProvider _serviceProvider;

        internal IServiceScope BeginScope()
        {
            return _serviceProvider.CreateScope();
        }
    }
}
