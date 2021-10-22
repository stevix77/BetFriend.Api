using BetFriend.Shared.Domain;
using System;

namespace BetFriend.Bet.Domain.Bets.Events
{
    public class BetUpdated : IDomainEvent
    {
        public BetUpdated(Guid betId)
        {
            BetId = betId;
        }

        public Guid BetId { get; }
    }
}
