namespace BetFriend.Domain.Bets.Events
{
    using BetFriend.Domain.Members;
    using System.Collections.Generic;

    public class BetCreated : IDomainEvent
    {
        public BetCreated(BetId betId, MemberId creatorId, IEnumerable<MemberId> participants)
        {
            BetId = betId;
            MemberId = creatorId;
            Participants = participants;
        }

        public BetId BetId { get; }
        public MemberId MemberId { get; }
        public IEnumerable<MemberId> Participants { get; }
    }
}
