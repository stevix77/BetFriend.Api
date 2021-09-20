﻿using BetFriend.Bet.Domain.Members;
using System;
using System.Collections.Generic;

namespace BetFriend.Bet.Domain.Bets
{
    public class BetState
    {
        public BetState(Guid betId,
                        Member creator,
                        DateTime endDate,
                        string description,
                        int tokens,
                        DateTime creationDate,
                        IReadOnlyCollection<AnswerState> answers, 
                        Status status = null)
        {
            BetId = betId;
            Creator = creator;
            EndDate = endDate;
            Description = description;
            Coins = tokens;
            CreationDate = creationDate;
            Answers = answers;
            Status = status;
        }

        public Guid BetId { get; }
        public Member Creator { get; }
        public DateTime EndDate { get; }
        public string Description { get; }
        public int Coins { get; }
        public Status Status { get; }
        public DateTime CreationDate { get; }
        public IReadOnlyCollection<AnswerState> Answers { get; }
    }
}
