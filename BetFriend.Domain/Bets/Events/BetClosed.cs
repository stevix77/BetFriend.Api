using System;

namespace BetFriend.Bet.Domain.Bets.Events
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
