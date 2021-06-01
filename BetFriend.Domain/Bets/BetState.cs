using System;

namespace BetFriend.Domain.Bets
{
    public class BetState
    {
        public BetState(Guid betId, Guid creatorId, DateTime endDate, string description, int tokens, DateTime creationDate)
        {
            BetId = betId;
            CreatorId = creatorId;
            EndDate = endDate;
            Description = description;
            Tokens = tokens;
            CreationDate = creationDate;
        }

        public Guid BetId { get; }
        public Guid CreatorId { get; }
        public DateTime EndDate { get; }
        public string Description { get; }
        public int Tokens { get; }
        public DateTime CreationDate { get; }
    }
}
