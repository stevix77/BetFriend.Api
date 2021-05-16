namespace BetFriend.Domain.Bets
{
    using BetFriend.Domain.Members;
    using System;
    using System.Collections.Generic;

    public class Bet
    {
        private BetId _betId;
        private DateTime _endDate;
        private MemberId[] _participants;
        private MemberId _creatorId;

        public Bet(BetId betId, DateTime endDate, MemberId[] participants, MemberId creatorId)
        {
            _betId = betId;
            _endDate = endDate;
            _participants = participants;
            _creatorId = creatorId;
        }

        public static Bet Create(BetId betId, MemberId creatorId, DateTime endDate, MemberId[] participants)
        {
            return new Bet(betId, endDate, participants, creatorId);
        }

        public BetId GetId() => _betId;
    }
}
