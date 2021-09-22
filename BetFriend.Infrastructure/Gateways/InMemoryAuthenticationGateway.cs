namespace BetFriend.Bet.Infrastructure.Gateways
{
    using BetFriend.Shared.Domain;
    using System;


    public class InMemoryAuthenticationGateway : IAuthenticationGateway
    {
        private Guid _memberId;

        public InMemoryAuthenticationGateway(Guid memberId)
        {
            _memberId = memberId;
        }

        public bool IsAuthenticated(Guid memberId)
        {
            return memberId.Equals(_memberId);
        }
    }
}
