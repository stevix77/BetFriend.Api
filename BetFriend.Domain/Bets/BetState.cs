using BetFriend.Domain.Members;
using System;
using System.Collections.Generic;

namespace BetFriend.Domain.Bets
{
    public class BetState
    {
        public BetState(Guid betId,
                        Member creator,
                        DateTime endDate,
                        string description,
                        int tokens,
                        DateTime creationDate,
                        IReadOnlyCollection<AnswerState> answers)
        {
            BetId = betId;
            Creator = creator;
            EndDate = endDate;
            Description = description;
            Coins = tokens;
            CreationDate = creationDate;
            Answers = answers;
        }

        public Guid BetId { get; }
        public Member Creator { get; }
        public DateTime EndDate { get; }
        public string Description { get; }
        public int Coins { get; }
        public DateTime CreationDate { get; }
        public IReadOnlyCollection<AnswerState> Answers { get; }
    }
}
