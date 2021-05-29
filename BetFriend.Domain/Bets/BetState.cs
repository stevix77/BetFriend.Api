using System;

namespace BetFriend.Domain.Bets
{
    public class BetState
    {
        public BetState(Guid betId, Guid memberId, DateTime endDate, string description, Guid[] participants)
        {
            BetId = betId;
            MemberId = memberId;
            EndDate = endDate;
            Description = description;
            Participants = participants;
        }

        public Guid BetId { get; }
        public Guid MemberId { get; }
        public DateTime EndDate { get; }
        public string Description { get; }
        public Guid[] Participants { get; }

        public override bool Equals(object obj)
        {
            return ((BetState)obj).BetId == BetId;
        }
    }
}
