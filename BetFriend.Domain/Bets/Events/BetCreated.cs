namespace BetFriend.Bet.Domain.Bets.Events
{
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Shared.Domain;

    public class BetCreated : IDomainEvent
    {
        public BetCreated(BetId betId, MemberId creatorId)
        {
            BetId = betId;
            CreatorId = creatorId;
        }

        public BetId BetId { get; }
        public MemberId CreatorId { get; }
    }
}
