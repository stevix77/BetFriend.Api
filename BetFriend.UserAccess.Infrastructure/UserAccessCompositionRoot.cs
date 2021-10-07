namespace BetFriend.UserAccess.Infrastructure
{
    using Microsoft.Extensions.DependencyInjection;
    using System;


    internal class UserAccessCompositionRoot
    {
        private readonly IServiceProvider _serviceProvider;
        internal UserAccessCompositionRoot(IServiceProvider provider)
        {
            _serviceProvider = provider;
        }

        internal IServiceScope BeginScope()
        {
            return _serviceProvider.CreateScope();
        }
    }
}
