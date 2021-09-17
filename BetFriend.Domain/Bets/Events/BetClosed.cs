using System;

namespace BetFriend.Domain.Bets.Events
{
    public class BetClosed : IDomainEvent
    {
        public BetClosed(Guid betId)
        {
            BetId = betId;
        }

        public Guid BetId { get; }
    }
}
