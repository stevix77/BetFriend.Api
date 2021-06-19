﻿namespace BetFriend.Application.Usecases.AnswerBet
{
    using BetFriend.Domain;
    using System;

    public class AnswerBetCommand
    {
        public AnswerBetCommand(Guid memberId, Guid betId, bool answer, IDateTimeProvider dateAnswer)
        {
            MemberId = memberId;
            BetId = betId;
            IsAccepted = answer;
            DateAnswer = dateAnswer;
        }

        public Guid MemberId { get; }
        public Guid BetId { get; }
        public bool IsAccepted { get; }
        public IDateTimeProvider DateAnswer { get; }
    }
}
