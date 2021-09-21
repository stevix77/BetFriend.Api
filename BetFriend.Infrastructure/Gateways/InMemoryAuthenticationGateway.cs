using BetFriend.Bet.Domain;
using System;

namespace BetFriend.Bet.Infrastructure.Gateways
{
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
