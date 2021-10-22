namespace BetFriend.Bet.Domain.Bets.Events
{
    using BetFriend.Shared.Domain;
    using System;

    public class BetClosed : IDomainEvent
    {
        public BetClosed(Guid betId, bool isSuccess)
        {
            BetId = betId;
            IsSuccess = isSuccess;
        }

        public Guid BetId { get; }
        public bool IsSuccess { get; }
    }
}
