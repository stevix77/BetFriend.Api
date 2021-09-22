namespace BetFriend.Bet.Domain.Bets.Events
{
    using BetFriend.Shared.Domain;
    using System;

    public class BetAnswered : IDomainEvent
    {
        public BetAnswered(Guid betId, Guid memberId, bool isAccepted)
        {
            BetId = betId;
            MemberId = memberId;
            IsAccepted = isAccepted;
        }

        public Guid BetId { get; }
        public Guid MemberId { get; }
        public bool IsAccepted { get; }
    }
}
