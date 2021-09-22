namespace BetFriend.Bet.Domain.Bets.Events
{
    using BetFriend.Shared.Domain;
    using System;

    public class BetClosed : IDomainEvent
    {
        public BetClosed(Guid betId)
        {
            BetId = betId;
        }

        public Guid BetId { get; }
    }
}
