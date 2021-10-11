namespace BetFriend.Bet.Infrastructure.Gateways
{
    using BetFriend.Shared.Domain;
    using System;

    public class InMemoryAuthenticationGateway : IAuthenticationGateway
    {
        private readonly bool _isAuthenticated;
        private readonly Guid _userId;

        public InMemoryAuthenticationGateway(bool isAuthenticated, Guid userId)
        {
            _isAuthenticated = isAuthenticated;
            _userId = userId;
        }

        public Guid UserId => _userId;

        public bool IsAuthenticated()
        {
            return _isAuthenticated;
        }
    }
}
