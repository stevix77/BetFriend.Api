namespace BetFriend.Domain.Bets.Events
{
    using BetFriend.Domain.Members;

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
